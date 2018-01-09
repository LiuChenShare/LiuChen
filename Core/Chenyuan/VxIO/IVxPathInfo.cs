using System.Collections.Generic;
using System.IO;

namespace Chenyuan.VxIO
{
    /// <summary>
    /// 虚拟路径信息接口定义
    /// </summary>
    public interface IVxPathInfo : IVxObjectInfo
    {
        /// <summary>
        /// 获取路径类型
        /// </summary>
        /// <remarks>当类型为unknown时，会依据场景的变化而变化</remarks>
        VxPathType PathType
        {
            get;
        }

        /// <summary>
        /// 当前对象是否已经存在
        /// </summary>
        bool Exists
        {
            get;
        }

        /// <summary>
        /// 是否文件对象
        /// </summary>
        /// <remarks>非Directory类型</remarks>
        bool IsFile
        {
            get;
        }

        /// <summary>
        /// 是否目录对象
        /// </summary>
        /// <remarks>非File类型</remarks>
        bool IsFolder
        {
            get;
        }

        /// <summary>
        /// 相对于Provider的路径字符串
        /// </summary>
        string PathString
        {
            get;
        }

        void Create(VxPathType folder);

        /// <summary>
        /// 相对于环境的完整路径字符串
        /// </summary>
        string FullPathString
        {
            get;
        }

        /// <summary>
        /// 父级路径对象
        /// </summary>
        /// <remarks>当当前为根时返回当前对象</remarks>
        IVxPathInfo Parent
        {
            get;
        }

        /// <summary>
        /// 相对于Provider的根路径对象
        /// </summary>
        IVxPathInfo Root
        {
            get;
        }

        /// <summary>
        /// 是否根对象
        /// </summary>
        bool IsRoot
        {
            get;
        }

        /// <summary>
        /// 获取物理路径信息
        /// </summary>
        string PhysicalPath
        {
            get;
        }

        /// <summary>
        /// 获取对象的名称
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取对象的扩展名
        /// </summary>
        string Extension
        {
            get;
        }

        /// <summary>
        /// 获取所有子级路径对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IEnumerable<IVxPathInfo> GetChildren(string pattern = null);

        /// <summary>
        /// 获取所有子级文件对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IEnumerable<IVxPathInfo> GetFiles(string pattern = null);

        /// <summary>
        /// 获取所有子级文件夹对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IEnumerable<IVxPathInfo> GetFolders(string pattern = null);

        /// <summary>
        /// 获取当前路径对象的目标文件夹对象
        /// </summary>
        /// <returns></returns>
        IVxIoObject GetVxIoObject();

        /// <summary>
        /// 获取当前路径对象的目标文件对象
        /// </summary>
        /// <returns></returns>
        IVxFileInfo GetFile();

        /// <summary>
        /// 获取目标文件夹对象
        /// </summary>
        /// <returns></returns>
        IVxFolderInfo GetFolder();

        /// <summary>
        /// 打开当前文件对象
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        Stream OpenFile(FileMode mode);

    }
}
