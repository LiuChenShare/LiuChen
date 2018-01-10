using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Chenyuan.ComponentModel;
using Chenyuan.Configuration;
using Chenyuan.Data;
using Chenyuan.Date;
using Chenyuan.Extensions;
using Chenyuan.Infrastructure;

namespace Chenyuan.Data.EF
{
	/// <summary>
	/// Entity Framework repository
	/// </summary>
	public abstract partial class EfRepositoryBase<T> : IChenyuanRepository<T> where T : BaseEntity
	{
		private IDbSet<T> _entities;
		private readonly bool _createEmptyEntityLog;
		private readonly bool _autoUpdateEntityLog;

		/// <summary>
		/// 构造函数
		/// </summary>
		public EfRepositoryBase()
		{
			this.AutoCommitEnabled = true;
			Type t = typeof(T);
			LogActivityConfig config = ChenyuanConfigManager.LogActivityConfig;
			if (config.LogEntities != null && config.LogEntities.Exists(x => x.Name == t.Name))
			{
				_autoUpdateEntityLog = true;
			}
			else
			{
				_createEmptyEntityLog = true;
			}
		}

		/// <summary>
		/// 获取数据上下文
		/// </summary>
		protected abstract IChenyuanDBContext DbContext
		{
			get;
		}

		private IEntityLogInfo<T> CreateEntityLogInfo(EntityLogActionType logAction, T entity, bool autoUpdate = true, string logActionName = null)
		{
			try
			{
				IEntityLogInfo<T> result;
				if (_createEmptyEntityLog)
				{
					result = EntityLogInfo<T>.CreateEmptyEntityLogInfo(entity);
				}
				else
				{
					result = new EntityLogInfoImpl<T>(logAction, entity, this.DbContext, logActionName);// EntityLogInfo<T>.CreateDefaultEntityLogInfo(logAction, entity);
				}
				if (autoUpdate && _autoUpdateEntityLog)
				{
					result.Update(this.AutoCommitEnabled);
				}
				return result;
			}
			catch
			{
				return EntityLogInfo<T>.CreateEmptyEntityLogInfo(entity);
			}
		}

		#region interface members

		/// <summary>
		/// 数据实体集合
		/// </summary>
		public virtual IQueryable<T> Table
		{
			get
			{
				return this.Entities.Where(x => !x.Deleted);
			}
		}

		/// <summary>
		/// 新建实体对象
		/// </summary>
		/// <returns></returns>
		public T Create()
		{
			return this.Entities.Create();
		}

		/// <summary>
		/// 根据Id获取数据对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public T GetById(object id)
		{
			var result = this.Entities.Find(id);
			if (result == null || result.Deleted)
			{
				return default(T);
			}
			return result;
		}

		/// <summary>
		/// 插入数据
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="autoLog"></param>
		/// <returns></returns>
		public IEntityLogInfo<T> Insert(T entity, bool autoLog = true)
		{
			Guard.ArgumentNotNull(() => entity);

			entity.CreatedOn = DateTime.Now;
			if (entity is BaseDataEntity)
			{
				(entity as BaseDataEntity).LastUpdatedOn = DateTime.Now;
			}
			this.Entities.Add(entity);

			if (this.AutoCommitEnabled)
				DbContext.SaveChanges();
			return CreateEntityLogInfo(EntityLogActionType.Create, entity, autoLog);
		}

		/// <summary>
		/// 批量插入数据
		/// </summary>
		/// <param name="entities"></param>
		/// <param name="batchSize"></param>
		/// <param name="autoLog"></param>
		/// <returns></returns>
		public IEnumerable<IEntityLogInfo<T>> InsertRange(IEnumerable<T> entities, int batchSize = 100, bool autoLog = true)
		{
			try
			{
				Guard.ArgumentNotNull(() => entities);
				entities.Each(x => { x.CreatedOn = DateTime.Now; /*x.LastUpdatedOn = null*/; });
				IList<IEntityLogInfo<T>> result = new List<IEntityLogInfo<T>>();
				if (entities.HasItems())
				{
					if (batchSize <= 0)
					{
						// insert all in one step
						entities.Each(x =>
						{
							this.Entities.Add(x);
							result.Add(CreateEntityLogInfo(EntityLogActionType.Create, x, autoLog));
						});
						if (this.AutoCommitEnabled)
							DbContext.SaveChanges();
					}
					else
					{
						int i = 1;
						bool saved = false;
						foreach (var entity in entities)
						{
							result.Add(this.CreateEntityLogInfo(EntityLogActionType.Create, entity, autoLog));
							this.Entities.Add(entity);
							saved = false;
							if (i % batchSize == 0)
							{
								if (this.AutoCommitEnabled)
									DbContext.SaveChanges();
								i = 0;
								saved = true;
							}
							i++;
						}

						if (!saved)
						{
							if (this.AutoCommitEnabled)
								DbContext.SaveChanges();
						}
					}
				}
				return result;
			}
			catch (DbEntityValidationException ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 更新数据
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="autoLog"></param>
		/// <param name="logAction"></param>
		/// <returns></returns>
		public IEntityLogInfo<T> Update(T entity, bool autoLog = true, string logAction = null)
		{
			Guard.ArgumentNotNull(() => entity);
			if (entity is BaseDataEntity)
			{
				(entity as BaseDataEntity).LastUpdatedOn = DateTime.Now;
			}
			var logInfo = CreateEntityLogInfo(EntityLogActionType.Update, entity, false, logAction);
			if (this.AutoCommitEnabled)
			{
				DbContext.SaveChanges();
			}
			else
			{
				try
				{
					this.Entities.Attach(entity);
					InternalContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
				}
				finally { }
			}
			if (autoLog && _autoUpdateEntityLog)
			{
				logInfo.Update(this.AutoCommitEnabled);
			}
			return logInfo;
		}

		/// <summary>
		/// 删除实体对象
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="autoLog"></param>
		/// <returns></returns>
		public IEntityLogInfo<T> Delete(T entity, bool autoLog = true)
		{
			Guard.ArgumentNotNull(() => entity);
			var logInfo = CreateEntityLogInfo(EntityLogActionType.Delete, entity, autoLog);

			if (InternalContext.Entry(entity).State == System.Data.Entity.EntityState.Detached)
			{
				this.Entities.Attach(entity);
			}

			this.Entities.Remove(entity);

			if (this.AutoCommitEnabled)
				DbContext.SaveChanges();
			return logInfo;
		}

		/// <summary>
		/// 展开实体对象（同时获取子集合）
		/// </summary>
		/// <param name="query"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public IQueryable<T> Expand(IQueryable<T> query, string path)
		{
			Guard.ArgumentNotNull(query, "query");
			Guard.ArgumentNotEmpty(path, "path");

			return query.Include(path);
		}

		/// <summary>
		/// 展开实体对象（同时获取子集合）
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="query"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public IQueryable<T> Expand<TProperty>(IQueryable<T> query, Expression<Func<T, TProperty>> path)
		{
			Guard.ArgumentNotNull(query, "query");
			Guard.ArgumentNotNull(path, "path");

			return query.Include(path);
		}

		/// <summary>
		/// 获取属性访问特性
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public IDictionary<string, object> GetModifiedProperties(T entity)
		{
			var props = new Dictionary<string, object>();

			var ctx = InternalContext;
			var entry = ctx.Entry(entity);
			var modifiedPropertyNames = from p in entry.CurrentValues.PropertyNames
										where entry.Property(p).IsModified
										select p;
			foreach (var name in modifiedPropertyNames)
			{
				props.Add(name, entry.Property(name).OriginalValue);
			}

			return props;
		}

		/// <summary>
		/// 数据上下文
		/// </summary>
		public IChenyuanDBContext Context
		{
			get { return DbContext; }
		}

		/// <summary>
		/// 是否自动提交数据
		/// </summary>
		public bool AutoCommitEnabled { get; set; }

		/// <summary>
		/// 实体集合名
		/// </summary>
		public string EntitySetName
		{
			get
			{
				return (this.Entities as IDbSet<T>).ElementType.Name;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procedureName"></param>
		/// <param name="parameters"></param>
		public virtual void ExecuteStoredProcedure(string procedureName, params object[] parameters)
		{
			this.DbContext.ExecuteStoredProcedure(procedureName, parameters);
		}

		#endregion

		#region Helpers

		/// <summary>
		/// 内置上下文对象
		/// </summary>
		protected internal ObjectContextBase InternalContext
		{
			get { return DbContext as ObjectContextBase; }
		}

		/// <summary>
		/// 实体集合
		/// </summary>
		protected DbSet<T> Entities
		{
			get
			{
				if (_entities == null)
				{
					_entities = DbContext.Set<T>();
				}
				return _entities as DbSet<T>;
			}
		}

		#endregion

	}
}
