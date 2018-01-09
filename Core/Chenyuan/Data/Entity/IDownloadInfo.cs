using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Entity
{
    public interface IDownloadInfo
    {
        Guid Id { get; set; }
        Guid? OperaterId { get; set; }
        Guid? LibraryId { get; set; }

        string FileName { get; set; }

        string PathKey { get; set; }
        /// <summary>
        /// oss返回的下载唯一标记ID
        /// 如果是本地则直接是当前表Id
        /// </summary>
        string DownloadID { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        long TotalSize { get; set; }
        /// <summary>
        /// 当前位置
        /// </summary>
        long Range { get; set; }
        /// <summary>
        /// 当前请求索引
        /// </summary>
        string DownloadedIndexs { get; set; }
        /// <summary>
        /// 下载文件标识
        /// </summary>
        string ETags { get; set; }
        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        DateTime LastModify { get; set; }
    }
}
