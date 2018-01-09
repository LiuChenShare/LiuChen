using System.IO;

namespace Chenyuan.VxIO.Vfs
{
    public class VfsPathInfo : VxPathInfo
    {
        public VfsPathInfo(string virtualPath, VfsProviderInfo virtualPathProvider)
            : base(virtualPath, virtualPathProvider)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        public new VfsProviderInfo Provider => base.Provider as VfsProviderInfo;

        public FileSystemInfo FileSystemInfo
        {
            get;
            set;
        }

        protected override Stream OnOpenFile(FileMode mode)
        {
            return (this.FileSystemInfo as FileInfo).Open(mode);
        }

        public sealed override bool IsFile => this.FileSystemInfo is FileInfo;

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public new IEnumerable<VfsFolderInfo> GetDirectories(string pattern) => base.GetDirectories(pattern).Cast<VfsFolderInfo>();

        //public new VfsFolderInfo GetDirectory() => base.GetDirectory() as VfsFolderInfo;

        //protected override IVxFolderInfo OnGetDirectory()
        //{
        //    return base.GetDirectory();
        //}

        //protected override IEnumerable<IVxFolderInfo> OnGetDirectories(string pattern)
        //{
        //    var directoryInfo = this.FileSystemInfo as DirectoryInfo;
        //    DirectoryInfo[] list = null;
        //    if (string.IsNullOrWhiteSpace(pattern))
        //    {
        //        list = directoryInfo.GetDirectories();
        //    }
        //    else
        //    {
        //        list = directoryInfo.GetDirectories(pattern);
        //    }
        //    return list.Select(x => new VfsPathInfo(this.VirtualPathProvider.CombinePathes(this.VxPathString, x.Name), this.VirtualPathProvider).GetDirectory()).ToArray();
        //}
    }
}
