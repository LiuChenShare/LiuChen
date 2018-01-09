using System;
using System.IO;

namespace Chenyuan.VxIO
{
    /// <summary>
    /// 虚拟文件对象类实现
    /// </summary>
    [Serializable]
    public class VxFileInfo : VxIoObject, IVxFileInfo
    {
        #region 构造过程

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="virtualPath"></param>
        public VxFileInfo(IVxPathInfo virtualPath)
            : base(virtualPath)
        {
        }

        /// <summary>
        /// 校验一个虚拟路径对象是否一个文件对象
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        protected sealed override IVxPathInfo VerifyVxPathObject(IVxPathInfo pathInfo)
        {
            if (pathInfo.Exists && !pathInfo.IsFile)
            {
                throw new InvalidOperationException($"path {pathInfo.PhysicalPath} must be a file object.");
            }
            return pathInfo;
        }

        /// <summary>
        /// 初始文件对象
        /// </summary>
        /// <param name="pathInfo"></param>
        protected override void OnInitialize(IVxPathInfo pathInfo)
        {
            if (!pathInfo.Exists)
            {
                if (pathInfo.Parent.Exists)
                {
                    pathInfo.Parent.Create(VxPathType.Folder);
                }
                pathInfo.Create(VxPathType.File);
            }
        }

        /// <summary>
        /// 强制为非文件夹
        /// </summary>
        public sealed override bool IsFolder => false;

        #endregion

        /// <summary>
        /// 打开文件流
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public virtual Stream Open(FileMode mode) => VxPathObject.OpenFile(mode);

        /// <summary>
        /// 获取所在文件夹对象
        /// </summary>
        public VxFolderInfo Folder => this.VxPathObject.Parent.GetFolder() as VxFolderInfo;

        IVxFolderInfo IVxFileInfo.Folder => this.Folder;
    }
}
