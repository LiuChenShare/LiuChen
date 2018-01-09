using System;

namespace Chenyuan.Date.Entity
{
    /// <summary>
    /// 实体ID接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IEntityId<TPrimaryKey>
        where TPrimaryKey : struct
    {
        /// <summary>
        /// 
        /// </summary>
        TPrimaryKey Id
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void SetId(TPrimaryKey id);

        /// <summary>
        /// 
        /// </summary>
        bool IsEmpty
        {
            get;
        }
    }

    /// <summary>
    /// 对象ID结构定义
    /// </summary>
    [Serializable]
    //[JsonConverter(typeof(EntityIdJsonConverter))]
    public struct EntityId<TPrimaryKey> : IEntityId<TPrimaryKey>
        where TPrimaryKey : struct
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        public EntityId(TPrimaryKey id)
        {
            this.Id = id;
        }

        #region 属性与方法

        /// <summary>
        /// 空对象
        /// </summary>
        public static EntityId<TPrimaryKey> Empty
        {
            get;
        } = new EntityId<TPrimaryKey>();

        /// <summary>
        /// 获取主键
        /// </summary>
        public TPrimaryKey Id
        {
            get; internal set;
        }

        /// <summary>
        /// 主键是否为空
        /// </summary>
        public bool IsEmpty => object.Equals(this.Id, default(TPrimaryKey));

        /// <summary>
        /// 设置主键
        /// </summary>
        /// <param name="id"></param>
        public void SetId(TPrimaryKey id) => this.Id = id;

        #endregion

        #region 重载方法

        /// <summary>
        /// 相等判断
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj is IEntityId<TPrimaryKey>)
            {
                return this.Equals((IEntityId<TPrimaryKey>)obj);
            }
            if (obj is TPrimaryKey)
            {
                return this.Equals((TPrimaryKey)obj);
            }
            return false;
        }

        /// <summary>
        /// 相等判断
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(IEntityId<TPrimaryKey> obj)
        {
            if (obj.IsEmpty)
            {
                return false;
            }
            return Equals(obj.Id, this.Id);
        }

        /// <summary>
        /// 相等判断
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(TPrimaryKey obj) => Equals(this.Id, obj);

        /// <summary>
        /// 获取HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Id.GetHashCode();

        /// <summary>
        /// 获取对象的字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Id}";

        #endregion

        #region 操作符重载

        /// <summary>
        /// 相等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(EntityId<TPrimaryKey> left, TPrimaryKey right) => Equals(left.Id, right);

        /// <summary>
        /// 不等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EntityId<TPrimaryKey> left, TPrimaryKey right) => !Equals(left.Id, right);

        /// <summary>
        /// 相等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(TPrimaryKey left, EntityId<TPrimaryKey> right) => Equals(right.Id, left);

        /// <summary>
        /// 不等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TPrimaryKey left, EntityId<TPrimaryKey> right) => !Equals(right.Id, left);

        /// <summary>
        /// 相等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(EntityId<TPrimaryKey> left, EntityId<TPrimaryKey> right) => Equals(left.Id, right.Id);

        /// <summary>
        /// 不等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EntityId<TPrimaryKey> left, EntityId<TPrimaryKey> right) => !Equals(left.Id, right.Id);

        /// <summary>
        /// 相等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(IEntityId<TPrimaryKey> left, EntityId<TPrimaryKey> right) => Equals(left.Id, right.Id);

        /// <summary>
        /// 不等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(IEntityId<TPrimaryKey> left, EntityId<TPrimaryKey> right) => !Equals(left.Id, right.Id);

        /// <summary>
        /// 相等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(EntityId<TPrimaryKey> left, IEntityId<TPrimaryKey> right) => Equals(left.Id, right.Id);

        /// <summary>
        /// 不等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EntityId<TPrimaryKey> left, IEntityId<TPrimaryKey> right) => !Equals(left.Id, right.Id);

        /// <summary>
        /// 转换为Guid
        /// </summary>
        /// <param name="objId"></param>
        public static implicit operator TPrimaryKey(EntityId<TPrimaryKey> objId) => objId.Id;

        /// <summary>
        /// 转换自Guid
        /// </summary>
        /// <param name="objId"></param>
        public static implicit operator EntityId<TPrimaryKey>(TPrimaryKey objId) => new EntityId<TPrimaryKey>(objId);

        #endregion

    }
}
