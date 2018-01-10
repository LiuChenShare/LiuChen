using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Chenyuan.ComponentModel;
using Chenyuan.Extensions;
using Chenyuan.Infrastructure;

namespace Chenyuan.Data
{
	/// <summary>
	/// 实体日志信息实现类
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class EntityLogInfoImpl<TEntity> : EntityLogInfo<TEntity>
		where TEntity : BaseEntity
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logAction"></param>
		/// <param name="entity"></param>
		/// <param name="dbContext"></param>
		/// <param name="logActionName"></param>
		public EntityLogInfoImpl(EntityLogActionType logAction, TEntity entity, IChenyuanDBContext dbContext = null, string logActionName = null)
			: base(logAction, entity, logActionName)
		{
			_dbContext = dbContext;
			LogInfo = EntityLogPropertyInfos(entity);
			if (logAction == EntityLogActionType.Update)
			{
				var entry = (_dbContext as DbContext).Entry(entity);
				this.OriginalValues = entry.OriginalValues.Clone();
			}
		}

		private EntityLogInfo LogInfo
		{
			get;
			set;
		}

		private DbPropertyValues OriginalValues
		{
			get;
			set;
		}

		#region method EntityLogPropertyInfos

		private class EntityLogInfo
		{
			public EntityLogInfo()
			{
				Attributes = new List<EntityLogAttribute>();
				Properties = new List<EntityPropertyLogInfo>();
			}

			public Type EntityType { get; set; }

			public IList<EntityLogAttribute> Attributes
			{
				get;
				private set;
			}

			public IList<EntityPropertyLogInfo> Properties
			{
				get;
				private set;
			}
		}

		private class EntityPropertyLogInfo
		{
			public EntityPropertyLogInfo()
			{
				Attributes = new List<EntityPropertyLogAttribute>();
			}

			public PropertyInfo Property { get; set; }

			public PropertyInfo ReferenceIdProperty { get; set; }

			public IList<EntityPropertyLogAttribute> Attributes
			{
				get;
				private set;
			}

			public EntityLogInfo Owner { get; set; }

			public Type PropertyType { get; set; }

			public bool Enumerable { get; set; }
		}

		private static IDictionary<Type, EntityLogInfo> s_EntityLogPropertyInfos;

		private static EntityLogInfo EntityLogPropertyInfos(BaseEntity entity)
		{
			return EntityLogPropertyInfos(entity.GetUnproxiedEntityType());
		}

		private static EntityLogInfo EntityLogPropertyInfos(Type entityType)
		{
			if (s_EntityLogPropertyInfos == null)
			{
				s_EntityLogPropertyInfos = new Dictionary<Type, EntityLogInfo>();
			}
			EntityLogInfo result = null;
			if (s_EntityLogPropertyInfos.ContainsKey(entityType))
			{
				result = s_EntityLogPropertyInfos[entityType];
			}
			else
			{
				s_EntityLogPropertyInfos.Add(entityType, result = new EntityLogInfo { EntityType = entityType });
				//result.Attributes.AddRange(entityType.GetCustomAttributes<EntityLogAttribute>(true));
				var properties = entityType.GetProperties();
				foreach (var property in properties)
				{
					//var customAttributes = property.GetCustomAttributes<EntityPropertyLogAttribute>(true);
					//if (!customAttributes.Any())
					//{
					//    continue;
					//}
					var propertyLogInfo = new EntityPropertyLogInfo { Property = property, Owner = result };
					//propertyLogInfo.Attributes.AddRange(customAttributes);
					bool isEnumerable = property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType);
					//propertyLogInfo.Enumerable = customAttributes.Where(x => x.Enumerable).Any() || isEnumerable;
					//propertyLogInfo.PropertyType = customAttributes.Where(x => x.PropertyType != null).Select(x => x.PropertyType).FirstOrDefault();
					if (propertyLogInfo.PropertyType == null)
					{
						if (propertyLogInfo.Enumerable)
						{
							if (property.PropertyType.IsCompatibleToGenericType(typeof(IEnumerable<>)))
							{
								propertyLogInfo.PropertyType = property.PropertyType.GetCompatibleGenericTypeParameterTypes(typeof(IEnumerable<>))[0];
							}
							else
							{
								propertyLogInfo.PropertyType = typeof(object);
							}
						}
						else
						{
							propertyLogInfo.PropertyType = property.PropertyType;
						}
					}
					//var referenceIdPropertyName = customAttributes.Where(x => x.ReferenceIdPropertyName.HasValue()).Select(x => x.ReferenceIdPropertyName).FirstOrDefault();
					//if (referenceIdPropertyName.HasValue())
					//{
					//    propertyLogInfo.ReferenceIdProperty = properties.Where(x => string.Equals(x.Name, referenceIdPropertyName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
					//}
					result.Properties.Add(propertyLogInfo);
				}
			}
			return result;
		}

		#endregion

		private IChenyuanDBContext _dbContext;
		/// <summary>
		/// 仓储数据操作对象
		/// </summary>
		protected virtual IChenyuanDBContext DbContext
		{
			get
			{
				if (_dbContext == null)
				{
					_dbContext = EngineContext.Current.Resolve<IChenyuanDBContext>();
				}
				return _dbContext;
			}
		}

		#region override methods or properties

		/// <summary>
		/// 
		/// </summary>
		protected override bool AutoLog
		{
			get
			{
				return LogInfo.Attributes.Select(x => x.AutoLog).Any();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="autoCommit"></param>
		protected override void Update(bool autoCommit)
		{
			//if (!this.LogInfo.Attributes.Any())
			//{
			//    //不存在特性定义，忽略本操作
			//    return;
			//}
			base.Update(autoCommit);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void Init()
		{
			switch (this.LogAction)
			{
				case EntityLogActionType.Create:
				case EntityLogActionType.Delete:
					this.BuildDataForCreateOrDelete();
					break;
				case EntityLogActionType.Unknown:
					this.Data.Value = "no data for logaction: {0}.".FormatWith(this.LogAction);
					break;
				case EntityLogActionType.Update:
				default:
					this.PrepareChangedData();
					break;
			}
		}

		#endregion

		private bool PrepareIEnumerableData(XElement data, IEnumerable items, string elementsName = "items")
		{
			var tmp = new XElement(elementsName);
			if (items == null)
			{
				return false;
			}
			bool flag = false;
			foreach (var item in items)
			{
				if (item == null)
				{
					continue;
				}
				object v;
				if (item is IEntityLogConverter)
				{
					v = (item as IEntityLogConverter).ToEntityLog();
				}
				else
				{
					v = item;
				}
				tmp.Add(new XElement("item", v));
				flag = true;
			}
			if (flag)
			{
				data.Add(tmp);
			}
			return flag;
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void BuildDataForCreateOrDelete()
		{
			var entity = this.Entity;
			var root = this.Data;
			XElement xProperty;
			var properties = LogInfo.Properties;
			foreach (var property in properties)
			{
				var value = property.Property.GetValue(entity);
				if (value == null)
				{
					if (property.ReferenceIdProperty != null)
					{
						try
						{
							value = this.GetValueByReferenceId(property.PropertyType, property.ReferenceIdProperty.GetValue(entity));
						}
						catch { }
					}
					if (value == null || value == (object)"")
					{
						continue;
					}
				}
				xProperty = new XElement(property.Property.Name);
				if (property.Enumerable)
				{
					if (!this.PrepareIEnumerableData(xProperty, value as IEnumerable, "values"))
					{
						continue;
					}
				}
				else
				{
					if (typeof(IEntityLogConverter).IsAssignableFrom(property.PropertyType))
					{
						XElement tmp;
						xProperty.Add(tmp = new XElement("value"));
						if (value != null)
						{
							tmp.Add(((IEntityLogConverter)value).ToEntityLog());
						}
					}
					else
					{
						xProperty.SetValue(value);
					}
				}
				root.Add(xProperty);
			}
		}

		private object GetValueByReferenceId(Type type, object referenceId)
		{
			if (type == null || referenceId == null)
			{
				return null;
			}
			if (type.IsEnum)
			{
				try
				{
					return Enum.ToObject(type, referenceId);
				}
				catch
				{
					return null;
				}
			}
			Type repositoryType = typeof(IChenyuanRepository<>);
			repositoryType = repositoryType.MakeGenericType(type);
			var method = repositoryType.GetMethod("GetById");
			object repository = null;
			try
			{
				repository = EngineContext.Current.Resolve(repositoryType);
			}
			catch { }
			if (method != null && repository != null)
			{
				return method.Invoke(repository, new object[] { referenceId });
			}
			return null;
		}

		private void PrepareChangedData(/*XElement root, BaseEntity entity, DbPropertyValues oldValues, DbPropertyValues newValues*/)
		{
			var entity = this.Entity;
			var root = this.Data;
			var oldValues = this.OriginalValues;
			XElement xProperty;
			var properties = LogInfo.Properties;
			foreach (var property in properties)
			{
				xProperty = new XElement(property.Property.Name);
				object @old = null;
				try
				{
					//读取原始数据
					@old = oldValues[property.Property.Name];
				}
				catch
				{
					//原始数据读取失败，从参考ID获取原始数据
					if (property.ReferenceIdProperty != null)
					{
						try
						{
							@old = this.GetValueByReferenceId(property.PropertyType, oldValues[property.ReferenceIdProperty.Name]);
						}
						catch { }
					}
				}
				object @new = property.Property.GetValue(entity);
				if (property.Enumerable)
				{
					if (!this.PrepareIEnumerableData(xProperty, @old as IEnumerable, "oldValues")
						&&
						!this.PrepareIEnumerableData(xProperty, @new as IEnumerable, "newValues"))
					{
						continue;
					}
				}
				else
				{
					if (object.Equals(@old, @new))
					{
						continue;
					}
					if (property.PropertyType == typeof(string) && string.Equals(@old as string, @new as string, StringComparison.CurrentCultureIgnoreCase))
					{
						continue;
					}
					bool flag = typeof(IEntityLogConverter).IsAssignableFrom(property.PropertyType);
					if (@old != null && @old != (object)"")
					{
						if (flag)
						{
							@old = ((IEntityLogConverter)@old).ToEntityLog();
						}
						xProperty.Add(new XElement("oldValue", @old));
					}
					if (@new != null && @old != (object)"")
					{
						if (flag)
						{
							@new = ((IEntityLogConverter)@new).ToEntityLog();
						}
						xProperty.Add(new XElement("newValue", @new));
					}
				}
				root.Add(xProperty);
			}
		}
	}
}
