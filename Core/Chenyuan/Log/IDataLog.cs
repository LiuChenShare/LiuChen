using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Data.Entity;
using Chenyuan.Date.Entity;

namespace Chenyuan.Log
{
    /// <summary>
    /// 数据日志
    /// </summary>
    public interface IDataLog
    {
        /// <summary>
        /// 实体日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="preObj"></param>
        /// <param name="afterObj"></param>
        /// <param name="description"></param>
        void EntityLog<T>(string entityId ,string preObj, string afterObj,string CRUD, string description = null) where T : EntityObject;
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="ex"></param>
        void ExceptionLog(Exception ex);
        /// <summary>
        /// 跟踪日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="Content"></param>
        void TraceLog(string message, string content);
        /// <summary>
        /// 是否需要日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool NeedLog<T>();
    }
}
