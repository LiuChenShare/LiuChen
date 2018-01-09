using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data
{
    public abstract class BaseDatabase
    {
        protected string path = "~/App_Data/";
        protected string _connString;

        public void LoadConnString(string filename)
        {
            try
            {
                path = System.Web.HttpContext.Current.Server.MapPath(path);
            }
            catch
            {
                string baseDirectory = AppContext.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                path = Path.Combine(baseDirectory, path);
            }
            string filePath = Path.Combine(path, filename);
            if (File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);
                StringReader sr = new StringReader(text);
                string provider = sr.ReadLine();
                string connectionString = sr.ReadLine();
                var arr = connectionString.Split(':');
                if (arr[0] == "DataConnectionString" && arr.Length == 2)
                {
                    _connString = arr[1];
                }
            }
        }
    }
}
