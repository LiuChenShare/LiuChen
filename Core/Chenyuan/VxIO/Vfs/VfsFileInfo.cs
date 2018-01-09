namespace Chenyuan.VxIO.Vfs
{
    public class VfsFileInfo : VxFileInfo
    {
        #region 重载与构造

        public VfsFileInfo(VfsPathInfo virtualPath)
            : base(virtualPath)
        {
        }

        public new VfsFolderInfo Folder => base.Folder as VfsFolderInfo;

        protected new VfsPathInfo VxPathObject => base.VxPathObject as VfsPathInfo;

        #endregion
    }
}
