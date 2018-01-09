using System;
using System.IO;

namespace Chenyuan.Enviroments
{
    class DefaultEnvironmentService : IEnvironmentService
    {
        public bool IsHosted => false;

        public string MapPath(string path)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }
    }
}
