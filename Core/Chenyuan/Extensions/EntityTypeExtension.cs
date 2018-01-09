using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Assemblies;
using Chenyuan.Data;
using Chenyuan.Extensions;
using Chenyuan.Infrastructure;

[assembly: PreApplicationStartMethod(typeof(EntityTypeExtension), "Initialize",true)]
namespace Chenyuan.Extensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class EntityTypeExtension
	{
		private static void Initialize()
		{
			BaseEntity.s_GetUnproxiedTypeHandler = (entity) => entity.GetUnproxiedEntityType();
		}

		/// <summary>
		/// Get unproxied entity type
		/// </summary>
		/// <remarks> If your Entity Framework context is proxy-enabled, 
		/// the runtime will create a proxy instance of your entities, 
		/// i.e. a dynamically generated class which inherits from your entity class 
		/// and overrides its virtual properties by inserting specific code useful for example 
		/// for tracking changes and lazy loading.
		/// </remarks>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static Type GetUnproxiedEntityType(this BaseEntity entity)
		{
			//string typeName = "System.Data.Entity.Core.Objects.ObjectContext, EntityFramework";
			//Type type = Type.GetType(typeName);
			//var m = type.GetMethod("GetObjectType");
			//return m.Invoke(null, new object[] { entity.GetType() }) as Type;
			//return type.InvokeMember("GetObjectType", BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[] { entity.GetType() }) as Type;
			var userType = ObjectContext.GetObjectType(entity.GetType());
			return userType;
		}

		#region 实体类型上下文键值处理

		private static readonly IDictionary<Type, string> s_entityTypeContextKeyMaps;
		static EntityTypeExtension()
		{
			s_entityTypeContextKeyMaps = new Dictionary<Type, string>();
		}

		/// <summary>
		/// 获取实体类型上下文键值集合
		/// </summary>
		public static IDictionary<Type, string> EntityTypeContextKeyMaps
		{
			get
			{
				return s_entityTypeContextKeyMaps;
			}
		}

		/// <summary>
		/// 添加实体类型上下文键值
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <param name="key">键值，不能为空</param>
		public static void AddEntityContextKey<TEntity>(string key)
			where TEntity : BaseEntity
		{
			AddEntityContextKeyInternal(typeof(TEntity), key);
		}

		/// <summary>
		/// 添加实体类型上下文键值
		/// </summary>
		/// <param name="entityType">实体类型，必须与类型 Chenyuan.Cor.Data.BaseEntity 兼容</param>
		/// <param name="key">键值，不能为空</param>
		public static void AddEntityContextKey(Type entityType, string key)
		{
			Guard.ArgumentNotNull(() => entityType);
			if (!typeof(BaseEntity).IsAssignableFrom(entityType))
			{
				throw new ArgumentOutOfRangeException("type: {0} must inherit from {1}.".FormatWith(entityType.FullName, typeof(BaseEntity).FullName));
			}
			AddEntityContextKeyInternal(entityType, key);
		}

		private static void AddEntityContextKeyInternal(Type entityType, string key)
		{
			Guard.ArgumentNotNull(() => key);
			Guard.ArgumentNotEmpty(() => key);
			if (s_entityTypeContextKeyMaps.ContainsKey(entityType))
			{
				s_entityTypeContextKeyMaps[entityType] = key;
			}
			else
			{
				s_entityTypeContextKeyMaps.Add(entityType, key);
			}
		}

		/// <summary>
		/// 检索指定实体的数据库上下文键
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <returns>实体类型上下文键值</returns>
		public static string GetEntityContextKey<TEntity>()
			where TEntity : BaseEntity
		{
			return GetEntityContextKeyInternal(typeof(TEntity));
		}

		/// <summary>
		/// 检索指定实体的数据库上下文键
		/// </summary>
		/// <param name="entityType">实体类型，必须与类型 Chenyuan.Cor.Data.BaseEntity 兼容</param>
		/// <returns>实体类型上下文键值</returns>
		public static string GetEntityContextKey(Type entityType)
		{
			Guard.ArgumentNotNull(() => entityType);
			if (!typeof(BaseEntity).IsAssignableFrom(entityType))
			{
				throw new ArgumentOutOfRangeException("type: {0} must inherit from {1}.".FormatWith(entityType.FullName, typeof(BaseEntity).FullName));
			}
			return GetEntityContextKeyInternal(entityType);
		}

		private static string GetEntityContextKeyInternal(Type entityType)
		{
			if (s_entityTypeContextKeyMaps.ContainsKey(entityType))
			{
				return s_entityTypeContextKeyMaps[entityType];
			}
			EntityContextKeyAttribute attribute = null;
			string contextKey = null;
			while (entityType != typeof(BaseEntity))
			{
				if (s_entityTypeContextKeyMaps.ContainsKey(entityType))
				{
					contextKey = s_entityTypeContextKeyMaps[entityType];
					break;
				}
				attribute = entityType.GetAttribute<EntityContextKeyAttribute>(false);
				if (attribute != null)
				{
					contextKey = attribute.ContextKey;
					break;
				}
				entityType = entityType.BaseType;
			}
			s_entityTypeContextKeyMaps.Add(entityType, contextKey);
			return contextKey;
		}

		#endregion
	}
}
