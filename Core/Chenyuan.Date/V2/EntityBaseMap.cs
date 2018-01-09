using Chenyuan.Date.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Date.V2
{
    public abstract class EntityBaseMap<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : EntityObject
    {
        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        public EntityBaseMap()
        {
            this.Initialization();
        }

        #endregion

        /// <summary>
        /// 初始化系统
        /// </summary>
        protected virtual void Initialization()
        {
            this.InitProperties();
            this.InitAssociations();
        }

        /// <summary>
        /// 初始化实体属性
        /// </summary>
        protected virtual void InitProperties()
        {
            this.Property(x => x.Timestamp)
                .IsRequired()
                .HasMaxLength(8)
                .IsRowVersion()
                .IsFixedLength();

            this.Property(x => x.CreatedOn)
                .IsRequired();

        }

        /// <summary>
        /// 初始化实体关系
        /// </summary>
        protected virtual void InitAssociations()
        {
        }

    }
}
