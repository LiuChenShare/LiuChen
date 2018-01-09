using System.IO;

namespace Chenyuan.VxIO.Vfs
{
    public sealed class VfsProviderInfo : VxProviderInfo
    {
        #region 构造

        public VfsProviderInfo(VxEnviroment enviroment, string physicalRoot, string relativeRoot = null)
            : base(enviroment, physicalRoot, relativeRoot)
        {
        }

        //public VfsProviderInfo(string root, IVxProviderInfo virtualPathProvider)
        //    : base(root, virtualPathProvider)
        //{
        //}

        //protected override IVxProviderInfo OnGetProvider(string root, string name)
        //{
        //    if (string.IsNullOrWhiteSpace(name))
        //    {
        //        return new VfsProviderInfo(root, ProviderRef);
        //    }
        //    return base.OnGetProvider(root, name);
        //}

        #endregion

        //public override string CombinePathes(string basePath, string relativePath, params string[] otherPathes)
        //{
        //    switch (otherPathes.Length)
        //    {
        //        case 0:
        //            return Path.Combine(basePath, relativePath);
        //        case 1:
        //            return Path.Combine(basePath, relativePath, otherPathes[0]);
        //        case 2:
        //            return Path.Combine(basePath, relativePath, otherPathes[0], otherPathes[1]);
        //    }
        //    string[] pathes = new string[otherPathes.Length + 2];
        //    pathes[0] = basePath;
        //    pathes[1] = relativePath;
        //    otherPathes.CopyTo(pathes, 2);
        //    return Path.Combine(pathes);
        //}

        //public new VfsPathInfo GetVirtualPath(string virtualPath) => base.GetVxPath(virtualPath) as VfsPathInfo;

        //protected override IVxPathInfo OnGetVxPath(string virtualPath) => new VfsPathInfo(virtualPath, this);

    }
}
