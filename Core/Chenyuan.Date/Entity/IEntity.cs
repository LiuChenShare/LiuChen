using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Date.Entity
{
    /// <summary>
    /// 实体接口定义
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体键值类型。可以是 <see cref="int"/>、<see cref="Guid"/> 等。</typeparam>
    public interface IEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {
        /// <summary>
        /// 实体主键
        /// </summary>
        IEntityId<TPrimaryKey> Id
        {
            get; set;
        }

        /// <summary>
        /// 是否临时对象（未持久化对象） <see cref="IEntityId{TPrimaryKey}"/>).
        /// </summary>
        /// <returns>当未持久化时为true</returns>
        bool IsTransient();
    }
}
