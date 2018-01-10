using Chenyuan.Date.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Base.Entity
{
    /// <summary>
    /// 晨源系统应用信息
    /// </summary>
    public class AppInfo : EntityBase<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 所属系统id
        /// </summary>

        public virtual Guid SystemId { get; set; }

        /// <summary>
        /// 父类id
        /// </summary>

        public virtual Guid? ParentId { get; set; }

        /// <summary>
        /// 层级深度
        /// </summary>

        public virtual int Deep { get; set; }

    }
}
