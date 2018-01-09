using System;
using System.Collections.Generic;
using System.Linq;

namespace Chenyuan.VxIO
{
    /// <summary>
    /// 虚拟文件夹对象类实现
    /// </summary>
    [Serializable]
    public class VxFolderInfo : VxIoObject, IVxFolderInfo
    {
        #region 构造过程

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="virtualPath"></param>
        public VxFolderInfo(IVxPathInfo virtualPath)
            : base(virtualPath)
        {
        }

        /// <summary>
        /// 校验一个虚拟路径对象是否文件夹对象
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        protected sealed override IVxPathInfo VerifyVxPathObject(IVxPathInfo pathInfo)
        {
            if (pathInfo.Exists && !pathInfo.IsFolder)
            {
                throw new InvalidOperationException($"path {pathInfo.PhysicalPath} must be a folder object.");
            }
            return pathInfo;
        }

        /// <summary>
        /// 初始化文件夹对象
        /// </summary>
        /// <param name="pathInfo"></param>
        protected override void OnInitialize(IVxPathInfo pathInfo)
        {
            if (!pathInfo.Exists)
            {
                pathInfo.Create(VxPathType.Folder);
            }
        }

        /// <summary>
        /// 强制IsFolder为文件夹
        /// </summary>
        public sealed override bool IsFolder => true;

        /// <summary>
        /// 是否根文件夹
        /// </summary>
        public bool IsRoot => this.VxPathObject.IsRoot;

        #endregion

        private VxFolderInfo _root;
        /// <summary>
        /// 获取根文件夹
        /// </summary>
        public VxFolderInfo Root
        {
            get
            {
                if (_root == null)
                {
                    _root = this.VxPathObject.Root.GetFolder() as VxFolderInfo;
                }
                return _root;
            }
        }

        private VxFolderInfo _parent;
        /// <summary>
        /// 获取父文件夹
        /// </summary>
        public VxFolderInfo Parent
        {
            get
            {
                if (this.VxPathObject.IsRoot)
                {
                    return this.Root;
                }
                if (_parent == null)
                {
                    _parent = this.VxPathObject.Parent.GetFolder() as VxFolderInfo;
                }
                return _parent;
            }
        }

        /// <summary>
        /// 获取所有子文件夹
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IEnumerable<VxFolderInfo> GetFolders(string pattern) => VxPathObject.GetFolders(pattern).Select(x => x.GetVxIoObject() as VxFolderInfo);

        /// <summary>
        /// 获取所有子文件
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IEnumerable<VxFileInfo> GetFiles(string pattern) => VxPathObject.GetFiles(pattern).Select(x => x.GetFile() as VxFileInfo);

        /// <summary>
        /// 获取所有子对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IEnumerable<VxIoObject> GetChildren(string pattern) => VxPathObject.GetChildren(pattern).Select(x => x.GetVxIoObject() as VxIoObject);

        #region 接口 IVxFolderInfo 显式实现

        IEnumerable<IVxFolderInfo> IVxFolderInfo.GetFolders(string pattern) => this.GetFolders(pattern);

        IEnumerable<IVxFileInfo> IVxFolderInfo.GetFiles(string pattern) => this.GetFiles(pattern);

        IEnumerable<IVxIoObject> IVxFolderInfo.GetChildren(string pattern) => this.GetChildren(pattern);

        IVxFolderInfo IVxFolderInfo.Root => this.Root;

        IVxFolderInfo IVxFolderInfo.Parent => this.Parent;

        #endregion
    }
}
