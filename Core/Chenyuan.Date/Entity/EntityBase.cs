using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chenyuan.Date.Entity
{
    /// <summary>
    /// 实体基类定义
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    [Serializable]
    public abstract class EntityBase<TPrimaryKey> : EntityObject, IEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TPrimaryKey Id
        {
            get
            {
                return this.OnGetId();
            }
            set
            {
                this.OnSetId(value);
            }
        }

        #region 属性与方法

        private EntityId<TPrimaryKey> _entityId = EntityId<TPrimaryKey>.Empty;
        /// <summary>
        /// 主键
        /// </summary>
        //[JsonConverter(typeof(EntityIdJsonConverter))]
        public virtual EntityId<TPrimaryKey> IdProxy
        {
            get
            {
                return _entityId;
            }
            set
            {
                _entityId.SetId(value.Id);
            }
        }

        IEntityId<TPrimaryKey> IEntity<TPrimaryKey>.Id
        {
            get
            {
                return this.IdProxy;
            }
            set
            {
                this.OnSetId(value.Id);
            }
        }

        /// <summary>
        /// 判断当前实体是否临时实体
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTransient() => _entityId.IsEmpty;

        /// <summary>
        /// 读取ID值
        /// </summary>
        /// <returns></returns>
        protected virtual TPrimaryKey OnGetId()
        {
            return _entityId.Id;
        }

        /// <summary>
        /// 写入ID值
        /// </summary>
        /// <param name="id"></param>
        protected virtual void OnSetId(TPrimaryKey id)
        {
            _entityId.SetId(id);
        }

        #endregion

        #region 重载函数

        /// <summary>
        /// 相等判断
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IEntity<TPrimaryKey>))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = (IEntity<TPrimaryKey>)obj;
            if (this.IsTransient() || other.IsTransient())
            {
                return false;
            }
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                return false;
            }
            return Equals(this.IdProxy, other.Id);
        }

        /// <summary>
        /// 获取HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => IdProxy.GetHashCode();

        /// <summary>
        /// 字符串表示形式：[{this.GetType().Name} {Id}]
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[{this.GetType().Name} {IdProxy}";

        /// <summary>
        /// 父类转子类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Parse<T>() where T : new()
        {
            T obj = new T();
            var ParentType = this.GetType();
            var Properties = ParentType.GetProperties();
            foreach (var Propertie in Properties)
            {
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    Propertie.SetValue(obj, Propertie.GetValue(this, null), null);
                }
            }
            return obj;
        }

        #endregion

        #region 操作符重载

        /// <summary>
        /// 相等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(EntityBase<TPrimaryKey> left, EntityBase<TPrimaryKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }
            return left.Equals(right);
        }

        /// <summary>
        /// 不等操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EntityBase<TPrimaryKey> left, EntityBase<TPrimaryKey> right) => !(left == right);

        #endregion
    }
}
