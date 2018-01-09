using System;
using System.Collections;
using System.Collections.Generic;
using Chenyuan.Caching.Defaults;

namespace Chenyuan.VxIO
{
    public interface IVxProviderInfo
    {
        /// <summary>
        /// 获取当前Provider对应的Enviroment对象
        /// </summary>
        IVxEnviroment Enviroment
        {
            get;
        }

        /// <summary>
        /// 获取当前Provider根路径
        /// </summary>
        IVxPathInfo Entry
        {
            get;
        }

        /// <summary>
        /// 获取指定对象的父对象
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        IVxPathInfo GetParent(IVxPathInfo pathInfo);

        /// <summary>
        /// 获取指定对象相对环境的完整路径
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        string GetFullPathString(IVxPathInfo pathInfo);

        /// <summary>
        /// 映射虚拟路径，获得实际的物理路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string MapPath(string path);

        /// <summary>
        /// 获取所有子级路径对象
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IEnumerable<IVxPathInfo> GetChildren(IVxPathInfo pathInfo, string pattern = null);

        /// <summary>
        /// 获取所有子级文件对象
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IEnumerable<IVxPathInfo> GetFiles(IVxPathInfo pathInfo, string pattern = null);

        /// <summary>
        /// 获取所有子级文件夹对象
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IEnumerable<IVxPathInfo> GetFolders(IVxPathInfo pathInfo, string pattern = null);

        //string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies);

        //CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart);

        //bool FileExists(string virtualPath);

        //bool DirectoryExists(string virtualDir);

        //IVxFileInfo GetFile(string virtualPath);

        //IVxDirectoryInfo GetDirectory(string virtualDir);

        //string GetCacheKey(string virtualPath);
        //IVxPathInfo GetVxPath(string virtualPath);
        //IVxPathInfo CreateFileVirtualPath(string virtualPath);

        //IVxPathInfo CreateDirectoryVirtualPath(string virtualPath);

        //string CombinePathes(string basePath, string relativePath, params string[] otherPathes);

        //IVxPathInfo CombinePathes(IVxPathInfo basePath, string relativePath, params string[] otherPathes);

        //IVxProviderInfo GetProvider<TProvider>(string root, string name = null)
        //    where TProvider : IVxProviderInfo;

        //IVxProviderInfo GetProvider(string root, string name = null);

    }
}
