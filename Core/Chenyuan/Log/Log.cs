using System;
using System.Collections.Generic;
using System.IO;

namespace Chenyuan.Log
{
    public abstract class Log
    {
        protected string LogFolder = "App_Logs";
        protected string LogFolder2 = "Base";
        protected string LogFilePath;
        /// <summary>
        /// 写日志
        /// </summary>
        protected virtual void WriteLog(LogInfo log)
        {
            if(string.IsNullOrEmpty(log.Path))
            {
                log.Path = LogFolder2;
            }
            string path = Path.Combine(AppContext.BaseDirectory, LogFolder, DateTime.Today.ToString("yyyy-MM-dd"),log.Path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string logname = $"{DateTime.Now.ToString("yyyyMMddHH")}_{log.Type.ToString()}.txt";
            string filePath = Path.Combine(path, logname);

            File.AppendAllLines(filePath, this.BuildLogContent(log));

            LogFilePath = filePath;
        }

        protected IEnumerable<string> BuildLogContent(LogInfo log)
        {
            IList<string> lines = new List<string>();
            lines.Add("===================Begin========================");
            lines.Add($"日志时间：{log.LogTime}");
            lines.Add($"请求URL：{log.RequestUrl}");
            lines.Add($"日志类型：{log.Type.ToString()}");
            lines.Add($"日志内容：{log.Content}");
            lines.Add("====================End========================");
            return lines;
        }
    }
}
