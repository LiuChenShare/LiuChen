using Chenyuan.Date.Entity;
using Chenyuan.Infrastructure;
using Chenyuan.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chenyuan.Data
{
    /// <summary>
	/// Base class for entities
	/// </summary>
	[Chenyuan.ComponentModel.GenericEntityOrProperty]
    [Serializable]
    public abstract partial class BaseEntity : EntityObject, IEntityLogConverter
    {
        /// <summary>
        /// 数据解包（获取原始类型）代理
        /// </summary>
        public static Func<BaseEntity, Type> s_GetUnproxiedTypeHandler = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseEntity()
        {
            this.RowGuid = Guid.NewGuid();
            this.Visibility = true;
            this.Enabled = true;
        }

        #region 属性
        /// <summary>
        /// 获取或设置数据实体标识(自增长)
        /// </summary>
        public long Id { get; set; }

        ///// <summary>
        ///// 获取数据流水Id（自增长id）
        ///// </summary>
        //public long IID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public long SubId { get; set; }
        /// <summary>
        /// 数据行唯一标识Guid
        /// </summary>
        public Guid RowGuid { get; set; }

        ///// <summary>
        ///// 获取或设置实体子Id，如果数据不需要，请重载该属性
        ///// </summary>
        //public virtual int SubId { get; set; }



        /// <summary>
        /// 长整型的位数据状态标志，如果数据不需要，请重载该属性
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 数据描述信息，如果数据不需要，请重载该属性
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        /// <LongDescription>
        /// 标识实体是否启用，默认为是
        /// </LongDescription>
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// 可见状态
        /// </summary>
        /// <LongDescription>
        /// 描述实体是否是可见的，如列表系统
        /// </LongDescription>
        public virtual bool Visibility { get; set; }

        #endregion

        #region 方法

        /// <summary>
        /// 判断两个对象是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        /// <summary>
        /// 判断对象是否临时数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool IsTransient(BaseEntity obj)
        {
            return obj != null && Equals(obj.Id, default(Guid));
        }

        /// <summary>
        /// 获取未被封包的数据类型
        /// </summary>
        /// <returns></returns>
        public Type GetUnproxiedType()
        {
            if (s_GetUnproxiedTypeHandler != null)
            {
                return s_GetUnproxiedTypeHandler(this);
            }
            try
            {
                string typeName = "System.Data.Entity.Core.Objects.ObjectContext, EntityFramework";
                Type type = Type.GetType(typeName);
                var m = type.GetMethod("GetObjectType");
                return m.Invoke(null, new object[] { this.GetType() }) as Type;
            }
            catch
            {
            }
            return GetType();
        }

        /// <summary>
        /// 判断两个对象是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        /// <summary>
        /// 获取对象的Hash值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (Equals(Id, 0))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        /// <summary>
        /// 实体令牌添加方法，由重载中实现
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns>返回结果，以支持cascade方式代码</returns>
        public virtual TokenCollection LoadTokens(TokenCollection tokens = null)
        {
            return tokens ?? new TokenCollection();
        }

        #endregion

        #region 操作符重载

        /// <summary>
        /// 判断两个对象是否相等
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// 判断两个对象是否不相等
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }

        #endregion

        #region 功能方法

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected TService ResolveService<TService>(string name = null)
            where TService : class
        {

            return EngineContext.Current.Resolve<TService>(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected object ResolveService(Type serviceType, string name = null)
        {
            return EngineContext.Current.Resolve(serviceType, name);
        }

        /// <summary>
        /// 设置对象的父对象
        /// </summary>
        /// <param name="child"></param>
        protected virtual void SetParent(dynamic child)
        {

        }

        /// <summary>
        /// 清空对象的父对象
        /// </summary>
        /// <param name="child"></param>
        protected virtual void SetParentToNull(dynamic child)
        {

        }

        /// <summary>
        /// 子数据集合处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="newCollection"></param>
        protected void ChildCollectionSetter<T>(ICollection<T> collection, ICollection<T> newCollection) where T : class
        {
            if (CommonHelper.Default.OneToManyCollectionWrapperEnabled)
            {
                collection.Clear();
                if (newCollection != null)
                    newCollection.ToList().ForEach(x => collection.Add(x));
            }
            else
            {
                collection = newCollection;
            }
        }

        /// <summary>
        /// 子数据集合处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="wrappedCollection"></param>
        /// <returns></returns>
        protected ICollection<T> ChildCollectionGetter<T>(ref ICollection<T> collection, ref ICollection<T> wrappedCollection) where T : class
        {
            return ChildCollectionGetter(ref collection, ref wrappedCollection, SetParent, SetParentToNull);
        }

        /// <summary>
        /// 子数据集合处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="wrappedCollection"></param>
        /// <param name="setParent"></param>
        /// <param name="setParentToNull"></param>
        /// <returns></returns>
        protected ICollection<T> ChildCollectionGetter<T>(ref ICollection<T> collection, ref ICollection<T> wrappedCollection, Action<dynamic> setParent, Action<dynamic> setParentToNull) where T : class
        {
            if (CommonHelper.Default.OneToManyCollectionWrapperEnabled)
                return wrappedCollection ?? (wrappedCollection = (collection ?? (collection = new List<T>())).SetupBeforeAndAfterActions(setParent, SetParentToNull));
            return collection ?? (collection = new List<T>());
        }

        /// <summary>
        /// 转换为实体日志XML内容
        /// </summary>
        /// <returns></returns>
        public virtual XElement ToEntityLog()
        {
            var result = new XElement("Value");
            result.Add(new XElement("entityType", this.GetUnproxiedType().Name));
            result.Add(new XElement("id", this.Id));
            return result;
        }

        /// <summary>
        /// 获取实体日子还字符串格式
        /// </summary>
        /// <returns></returns>
        public virtual string ToEntityLogString()
        {
            return this.Id.ToString();
        }

        #endregion
    }
}
