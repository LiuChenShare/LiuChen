using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan
{
    public interface IAppInfo
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 应用标题
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// 应用编码
        /// </summary>
        string Code { get; set; }
        /// <summary>
        /// 应用主机名
        /// </summary>
        string Entry { get; set; }

        /// <summary>
        /// 应用主机名
        /// </summary>
        string Entries { get; set; }
    }
}
