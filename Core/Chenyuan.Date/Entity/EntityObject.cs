using System;

namespace Chenyuan.Date.Entity
{
    /// <summary>
    /// 实体对象类
    /// </summary>
    [Serializable]
    public abstract class EntityObject// : AopObject
    {
        public EntityObject()
        {
            CreatedOn = DateTime.Now;
            LastUpdatedOn = DateTime.Now;
        }
        /// <summary>
        /// 数据删除状态
        /// </summary>
        public bool Deleted
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn
        {
            get;
            set;
        }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime LastUpdatedOn
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Timestamp
        {
            get;
            set;
        }
    }
}
