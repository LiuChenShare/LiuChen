using Chenyuan.Date.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Base.Entity
{
    /// <summary>
    /// 晨源系统子系统信息
    /// </summary>
    public class SystemInfo : EntityBase<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
    }
}
