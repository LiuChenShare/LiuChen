using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Chenyuan.Data;
using Chenyuan.Infrastructure;
using Chenyuan.Services;

namespace Chenyuan.ComponentModel
{
	// <summary>
	/// 实体日志信息基类定义
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public abstract class EntityLogInfo<TEntity> : IEntityLogInfo<TEntity>
		where TEntity : BaseEntity
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="logAction">实体操作行为</param>
		/// <param name="entity">实体对象</param>
		/// <param name="logActionName"></param>
		public EntityLogInfo(EntityLogActionType logAction, TEntity entity, string logActionName = null)
		{
			Guard.ArgumentNotNull(() => entity);
			Entity = entity;
			this.LogAction = logAction;
			_logActionName = logActionName;
			switch (this.LogAction)
			{
				case EntityLogActionType.Unknown:
					_updated = true;
					this.Data = new XElement("data", "not supported.");
					break;
				default:
					this.Data = new XElement("data");
					break;
			}
		}

		private bool _updated = false;
		private bool _inited = false;
		private IEntityLogService _logService;
		/// <summary>
		/// 实体日志执行服务
		/// </summary>
		protected virtual IEntityLogService LogService
		{
			get
			{
				if (_logService == null)
				{
					_logService = EngineContext.Current.Resolve<IEntityLogService>();
				}
				return _logService;
			}
		}

		/// <summary>
		/// 执行跟新操作
		/// </summary>
		protected virtual void Update(bool autoCommit)
		{
			this.InitInternal();
			switch (this.LogAction)
			{
				case EntityLogActionType.Create:
					LogService.Create(this, autoCommit);
					break;
				case EntityLogActionType.Update:
					LogService.Update(this, autoCommit);
					break;
				case EntityLogActionType.Delete:
					LogService.Delete(this, autoCommit);
					break;
			}
		}

		private void InitInternal()
		{
			if (!_inited)
			{
				_inited = true;
				//this.Data = new XElement("data");
				this.Init();
			}
		}

		/// <summary>
		/// 执行数据初始化
		/// </summary>
		protected virtual void Init()
		{

		}

		/// <summary>
		/// 获取实体对象
		/// </summary>
		protected TEntity Entity
		{
			get;
			private set;
		}

		/// <summary>
		/// 获取实体对象日志类型
		/// </summary>
		protected EntityLogActionType LogAction
		{
			get;
			private set;
		}

		/// <summary>
		/// 当前对象是否已经更新了日志
		/// </summary>
		protected bool Updated
		{
			get
			{
				return _updated;
			}
		}

		/// <summary>
		/// 当前数据是否已经初始化完成
		/// </summary>
		protected bool Inited
		{
			get
			{
				return _inited;
			}
		}

		/// <summary>
		/// 获取日志数据内容
		/// </summary>
		protected virtual XElement Data
		{
			get;
			private set;
		}

		/// <summary>
		/// 判断对象是否为空
		/// </summary>
		protected virtual bool IsEmpty
		{
			get
			{
				return this.LogAction == EntityLogActionType.Unknown;
			}
		}

		/// <summary>
		/// 是否自动执行日志记录行为
		/// </summary>
		protected virtual bool AutoLog
		{
			get
			{
				return false;
			}
		}

		private string _logActionName;
		/// <summary>
		/// 日志记录行为名
		/// </summary>
		protected virtual string LogActionName
		{
			get
			{
				return _logActionName;
			}
		}

		#region 接口实现

		void IEntityLogInfo<TEntity>.Update(bool autoCommit)
		{
			if (!_updated)
			{
				_updated = true;
				try
				{
					this.Update(autoCommit);
				}
				catch { }
			}
		}

		//void IEntityLogInfo<TEntity>.Init()
		//{
		//    if (!_inited)
		//    {
		//        _inited = true;
		//        this.Init();
		//    }
		//}

		bool IEntityLogInfo<TEntity>.Updated
		{
			get
			{
				return this.Updated;
			}
		}

		TEntity IEntityLogInfo<TEntity>.Entity
		{
			get
			{
				return this.Entity;
			}
		}

		EntityLogActionType IEntityLogInfo<TEntity>.LogAction
		{
			get
			{
				return this.LogAction;
			}
		}

		XElement IEntityLogInfo<TEntity>.Data
		{
			get
			{
				return this.Data;
			}
		}

		bool IEntityLogInfo<TEntity>.IsEmpty
		{
			get
			{
				return this.IsEmpty;
			}
		}

		bool IEntityLogInfo<TEntity>.AutoLog
		{
			get
			{
				return this.AutoLog;
			}
		}

		string IEntityLogInfo<TEntity>.LogActionName
		{
			get
			{
				return this.LogActionName ?? this.LogAction.ToString();
			}
		}

		#endregion

		#region Default EntityLogInfo

		///// <summary>
		///// 创建一个默认的实体日志对象
		///// </summary>
		///// <param name="logAction">日志行为</param>
		///// <param name="entity">日志对象</param>
		///// <returns></returns>
		//public static IEntityLogInfo<TEntity> CreateDefaultEntityLogInfo(EntityLogActionType logAction, TEntity entity)
		//{
		//    return new DefaultEntityLogInfo(logAction, entity);
		//}

		/// <summary>
		/// 构建一个空的日志信息对象
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static IEntityLogInfo<TEntity> CreateEmptyEntityLogInfo(TEntity entity)
		{
			return new DefaultEntityLogInfo(EntityLogActionType.Unknown, entity);
		}

		private class DefaultEntityLogInfo : EntityLogInfo<TEntity>
		{
			public DefaultEntityLogInfo(EntityLogActionType logAction, TEntity entity)
				: base(logAction, entity)
			{
			}

			
		}

		#endregion
	}
}
