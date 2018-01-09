using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Entity
{
    public interface IUploadInfo
    {
        /// <summary>
        /// 类型编码
        /// </summary>
        string DatumTypeCode { get; set; }

        /// <summary>
        /// 当前素材的格式
        /// </summary>
        string Format { get; set; }
        /// <summary>
        /// 素材名称
        /// 如果是文本，该字段就是文本内容
        /// </summary>
        string DatumName { get; set; }
        /// <summary>
        /// 文档存储路径key
        /// </summary>
        string PathKey { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// 标记上传唯一性的ID
        /// 如果是本地上传，则该值就是Id的值
        /// 如果是上传到OSS，则该值是OSS返回的唯一UploadId
        /// </summary>
        string UploadID { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        Guid? OperaterId { get; set; }
        /// <summary>
        /// 总片数
        /// </summary>
        int Total { get; set; }
        /// <summary>
        /// 已完成片数
        /// </summary>
        int CompleteCounts { get; set; }
        /// <summary>
        /// 已上传的文件片索引,逗号间隔
        /// </summary>
        string UploadedIndexs { get; set; }
        /// <summary>
        /// 分片上传状态序列化值
        /// </summary>
        string PartETags { get; set; }
        /// <summary>
        /// 上传开始时间
        /// </summary>
        DateTime BeginTime { get; set; }
        /// <summary>
        /// 上传结束时间
        /// </summary>
        DateTime? EndTime { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        long Size { get; set; }
    }
}
