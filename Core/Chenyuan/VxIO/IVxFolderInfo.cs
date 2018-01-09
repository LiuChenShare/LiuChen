using System.Collections.Generic;

namespace Chenyuan.VxIO
{
    /// <summary>
    /// 虚拟文件夹接口定义
    /// </summary>
    public interface IVxFolderInfo : IVxIoObject
    {
        /// <summary>
        /// 获取所有子文件夹对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IEnumerable<IVxFolderInfo> GetFolders(string pattern);

        /// <summary>
        /// 获取所有文件对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IEnumerable<IVxFileInfo> GetFiles(string pattern);

        /// <summary>
        /// 获取所有子对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IEnumerable<IVxIoObject> GetChildren(string pattern);

        /// <summary>
        /// 是否根文件夹
        /// </summary>
        bool IsRoot
        {
            get;
        }

        /// <summary>
        /// 获取根文件夹对象
        /// </summary>
        IVxFolderInfo Root
        {
            get;
        }

        /// <summary>
        /// 获取父文件夹对象
        /// </summary>
        IVxFolderInfo Parent
        {
            get;
        }
    }
}
