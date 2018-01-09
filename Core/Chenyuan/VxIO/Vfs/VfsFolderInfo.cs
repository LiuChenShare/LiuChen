using System.Collections.Generic;
using System.Linq;

namespace Chenyuan.VxIO.Vfs
{
    public class VfsFolderInfo : VxFolderInfo
    {
        #region 重载与构造

        public VfsFolderInfo(VfsPathInfo virtualPath)
            : base(virtualPath)
        {
        }

        protected new VfsPathInfo VxPathObject => base.VxPathObject as VfsPathInfo;

        //protected virtual VfsFolderInfo CreateDirectory(VfsPathInfo virtualPath) => new VfsFolderInfo(virtualPath);

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected sealed override IVxFolderInfo CreateDirectory(IVxPathInfo virtualPath) => this.CreateDirectory(virtualPath as VfsPathInfo);

        //public new IEnumerable<VfsFolderInfo> GetDirectories(string pattern) => base.GetDirectories(pattern).Cast< VfsFolderInfo>();

        public new IEnumerable<VfsFileInfo> GetFiles(string pattern) => base.GetFiles(pattern).Cast<VfsFileInfo>();

        //public new IEnumerable<VfsFolderInfo> Directories => base.Directories.Cast<VfsFolderInfo>();

        //public new IEnumerable<VfsFileInfo> Files => base.Files.Cast<VfsFileInfo>();

        public new VfsFolderInfo Parent => base.Parent as VfsFolderInfo;

        public new VfsFolderInfo Root => base.Root as VfsFolderInfo;

        #endregion
    }
}
