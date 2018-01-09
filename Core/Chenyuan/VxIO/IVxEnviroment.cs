using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.VxIO
{
    public interface IVxEnviroment
    {
        ///// <summary>
        ///// 组合路径
        ///// </summary>
        ///// <param name="path1"></param>
        ///// <param name="path2"></param>
        ///// <param name="otherPathes"></param>
        ///// <returns></returns>
        //string CombinePath(string path1, string path2, params string[] otherPathes);

        ///// <summary>
        ///// 组合路径
        ///// </summary>
        ///// <param name="path1"></param>
        ///// <param name="path2"></param>
        ///// <param name="otherPathes"></param>
        ///// <returns></returns>
        //IVxPathInfo CombinePath(IVxPathInfo path1, string path2, params string[] otherPathes);

        ///// <summary>
        ///// 创建目录
        ///// </summary>
        ///// <param name="pathInfo"></param>
        ///// <returns></returns>
        //bool CreateDirectory(IVxPathInfo pathInfo);

        ///// <summary>
        ///// 打开文件
        ///// </summary>
        ///// <param name="pathInfo"></param>
        ///// <param name="mode"></param>
        ///// <param name="stream"></param>
        ///// <returns></returns>
        //bool OpenFile(IVxPathInfo pathInfo, FileMode mode, out Stream stream);
        bool CaseSensetive
        {
            get;
        }

        char SeparatorChar
        {
            get;
        }

        IEnumerable<char> SeparatorChars
        {
            get;
        }

        /// <summary>
        /// 映射虚拟路径，获得实际的物理路径
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        string MapPath(IVxProviderInfo provider, string path);

        /// <summary>
        /// 分段路径数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ArraySegment<string> SegPath(string path);
    }
}
