using System;
using System.Collections.Generic;
using System.Linq;

namespace Chenyuan.VxIO
{
    public class VxProviderInfo : MarshalByRefObject, IVxProviderInfo
    {
        #region 构造

        public VxProviderInfo(VxEnviroment enviroment, string physicalRoot, string relativeRoot = null)
        {
            if (enviroment == null)
            {
                throw new ArgumentNullException(nameof(enviroment));
            }
            this.Enviroment = enviroment;
            this.EntryPathString = this.VerifyRelativeRoot(enviroment, relativeRoot);
            this.Entry = this.OnCreatePathInfo($"{enviroment.SeparatorChar}");
        }

        /// <summary>
        /// 校验路径信息
        /// </summary>
        /// <param name="enviroment"></param>
        /// <param name="relativeRoot"></param>
        /// <returns></returns>
        protected virtual string VerifyRelativeRoot(IVxEnviroment enviroment, string relativeRoot)
        {
            if (string.IsNullOrWhiteSpace(relativeRoot))
            {
                relativeRoot = $"{enviroment.SeparatorChar}";
            }
            else
            {
                if (!enviroment.SeparatorChars.Contains(relativeRoot[0]))
                {
                    relativeRoot = $"{enviroment.SeparatorChar}{relativeRoot}";
                }
                if (!enviroment.SeparatorChars.Contains(relativeRoot[relativeRoot.Length - 1]))
                {
                    relativeRoot = $"{relativeRoot}{enviroment.SeparatorChar}";
                }
            }
            return relativeRoot;
        }

        /// <summary>
        /// 创建路径对象
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        protected virtual VxPathInfo OnCreatePathInfo(string virtualPath) => null;

        #endregion

        #region public

        #region public methods

        /// <summary>
        /// 获取父对象
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        public VxPathInfo GetParent(VxPathInfo pathInfo)
        {
            if (pathInfo == null)
            {
                throw new ArgumentNullException(nameof(pathInfo));
            }
            return this.OnGetParent(pathInfo);
        }

        /// <summary>
        /// 获取指定对象相对环境的完整路径
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        public string GetFullPathString(VxPathInfo pathInfo) => this.OnGetFullPathString(pathInfo);

        /// <summary>
        /// 映射虚拟路径，获得实际的物理路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string MapPath(string path) => this.OnMapPath(path);

        /// <summary>
        /// 获取所有子级文件
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IEnumerable<VxPathInfo> GetFiles(VxPathInfo pathInfo, string pattern) => this.OnGetFiles(pathInfo, pattern);

        /// <summary>
        /// 获取所有子级文件夹
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IEnumerable<VxPathInfo> GetFolders(VxPathInfo pathInfo, string pattern) => this.OnGetFolders(pathInfo, pattern);

        /// <summary>
        /// 获取所有子级对象
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IEnumerable<VxPathInfo> GetChildren(VxPathInfo pathInfo, string pattern) => this.OnGetChildren(pathInfo, pattern);

        #endregion

        #region public properties

        /// <summary>
        /// 环境信息对象
        /// </summary>
        public VxEnviroment Enviroment
        {
            get;
        }

        /// <summary>
        /// 根路径
        /// </summary>
        public string EntryPathString
        {
            get;
        }

        /// <summary>
        /// 当前入口路径对象
        /// </summary>
        public VxPathInfo Entry
        {
            get;
        }

        #endregion

        #endregion

        #region protected

        #region protected methods

        /// <summary>
        /// 获取父对象
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        protected virtual VxPathInfo OnGetParent(VxPathInfo pathInfo)
        {
            if (pathInfo.IsRoot)
            {
                return pathInfo;
            }
            return this.OnCreatePathInfo(pathInfo.GetParentPathString());
        }

        /// <summary>
        /// 获取指定对象相对环境的完整路径
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        protected virtual string OnGetFullPathString(VxPathInfo pathInfo)
        {
            return $"{this.EntryPathString}{pathInfo.PathString.Substring(1)}";
        }

        /// <summary>
        /// 映射虚拟路径，获得实际的物理路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual string OnMapPath(string path)
        {
            return this.Enviroment.MapPath(this, path);
        }

        /// <summary>
        /// 获取所有子级文件
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        protected virtual IEnumerable<VxPathInfo> OnGetFiles(VxPathInfo pathInfo, string pattern)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有子级文件夹
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        protected virtual IEnumerable<VxPathInfo> OnGetFolders(VxPathInfo pathInfo, string pattern)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有子级对象
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        protected virtual IEnumerable<VxPathInfo> OnGetChildren(VxPathInfo pathInfo, string pattern)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region protected properties

        #endregion

        #endregion

        #region IVxProviderInfo 显式实现

        IVxPathInfo IVxProviderInfo.Entry => Entry;

        IVxEnviroment IVxProviderInfo.Enviroment => this.Enviroment;

        IEnumerable<IVxPathInfo> IVxProviderInfo.GetChildren(IVxPathInfo pathInfo, string pattern) => this.GetChildren(pathInfo as VxPathInfo, pattern);

        IEnumerable<IVxPathInfo> IVxProviderInfo.GetFiles(IVxPathInfo pathInfo, string pattern) => this.GetFiles(pathInfo as VxPathInfo, pattern);

        IEnumerable<IVxPathInfo> IVxProviderInfo.GetFolders(IVxPathInfo pathInfo, string pattern) => this.GetFolders(pathInfo as VxPathInfo, pattern);

        IVxPathInfo IVxProviderInfo.GetParent(IVxPathInfo pathInfo) => this.GetParent(pathInfo as VxPathInfo);

        string IVxProviderInfo.GetFullPathString(IVxPathInfo pathInfo) => this.GetFullPathString(pathInfo as VxPathInfo);

        #endregion
    }
}
