using System;

namespace Chenyuan.Log
{
    public class LogInfo
    {
        /// <summary>
        /// 日志文件夹
        /// </summary>
        public virtual string Path { get; set; }
        /// <summary>
        /// 日志产生时间
        /// </summary>
        public virtual DateTime LogTime { get; set; }
        /// <summary>
        /// 请求url
        /// </summary>
        public virtual string RequestUrl { get; set; }
        /// <summary>
        /// 日志内容类型
        /// </summary>
        public virtual LogType Type { get; set; }
        /// <summary>
        /// 日志详情
        /// </summary>
        public virtual string Content { get; set; }
        
    }
}
