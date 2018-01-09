using System.IO;

namespace Chenyuan.VxIO
{
    /// <summary>
    /// 虚拟文件接口定义
    /// </summary>
    public interface IVxFileInfo : IVxIoObject
    {
        /// <summary>
        /// 打开文件对象
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        Stream Open(FileMode mode);

        /// <summary>
        /// 获取文件所在文件夹对象
        /// </summary>
        IVxFolderInfo Folder
        {
            get;
        }
    }
}
