using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chenyuan.VxIO
{
    /// <summary>
    /// 虚拟路径信息类实现
    /// </summary>
    [Serializable]
    public class VxPathInfo : VxObjectInfo, IVxPathInfo
    {
        #region 构造

        /// <summary>
        /// 分段的路径字符串
        /// </summary>
        protected internal readonly ArraySegment<string> _segedPathInfo;
        /// <summary>
        /// 对象hash值
        /// </summary>
        protected readonly int _hashCode;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="provider"></param>
        public VxPathInfo(string virtualPath, VxProviderInfo provider)
        {
            if (string.IsNullOrWhiteSpace(virtualPath))
            {
                throw new ArgumentNullException(nameof(virtualPath));
            }
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            Provider = provider;
            _segedPathInfo = provider.Enviroment.SegPath(virtualPath);
            this.PathString = this.BuildPathString(_segedPathInfo, Provider.Enviroment.SeparatorChar);
            int hashProvider = provider.GetHashCode();
            int hashPath = 0;
            if (Provider.Enviroment.CaseSensetive)
            {
                hashPath = this.PathString.GetHashCode();
            }
            else
            {
                hashPath = this.PathString.ToUpper().GetHashCode();
            }
            _hashCode = hashProvider << 8 + hashPath;
            this.OnInitialize();
        }

        /// <summary>
        /// 初始化处理
        /// </summary>
        protected virtual void OnInitialize()
        {
        }

        /// <summary>
        /// 获取父路径信息
        /// </summary>
        /// <returns></returns>
        public string GetParentPathString()
        {
            char separatorChar = this.Provider.Enviroment.SeparatorChar;
            switch (_segedPathInfo.Count)
            {
                case 0:
                case 1:
                    return $"{separatorChar}";
                case 2:
                    return $"{separatorChar}{_segedPathInfo.Array[0]}";
            }
            return this.BuildPathString(new ArraySegment<string>(_segedPathInfo.Array, 0, _segedPathInfo.Count - 1), separatorChar);
        }

        /// <summary>
        /// 获取子对象路径信息
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public string GetChildPathString(string child)
        {
            if (string.IsNullOrWhiteSpace(child))
            {
                throw new ArgumentNullException(nameof(child));
            }
            char separatorChar = this.Provider.Enviroment.SeparatorChar;
            if (_segedPathInfo.Count == 0)
            {
                return $"{separatorChar}{child.Trim()}";
            }
            return this.BuildPathString(new ArraySegment<string>(_segedPathInfo.Array.Union(new string[] { child.Trim() }).ToArray(), 0, _segedPathInfo.Count + 1), separatorChar);
        }

        /// <summary>
        /// 依据字符串数组构建路径字符串
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="separatorChar"></param>
        /// <returns></returns>
        protected string BuildPathString(ArraySegment<string> segment, char separatorChar)
        {
            string result = $"{separatorChar}";
            if (segment.Count > 0)
            {
                result += string.Join($"{separatorChar}", segment);
            }
            return result;
        }

        #endregion

        #region public

        #region public methods

        /// <summary>
        /// 获取所有子级路径对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IEnumerable<VxPathInfo> GetChildren(string pattern) => this.OnGetChildren(pattern);

        /// <summary>
        /// 获取所有子级文件对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IEnumerable<VxPathInfo> GetFiles(string pattern) => this.OnGetFiles(pattern);

        /// <summary>
        /// 获取所有子级文件夹对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IEnumerable<VxPathInfo> GetFolders(string pattern) => this.OnGetFolders(pattern);

        /// <summary>
        /// 打开文件流
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Stream OpenFile(FileMode mode) => this.OnOpenFile(mode);

        /// <summary>
        /// 创建路径对象
        /// </summary>
        /// <param name="folder"></param>
        public void Create(VxPathType folder)
        {
            switch (folder)
            {
                case VxPathType.File:
                    this.OnCreateFile();
                    break;
                case VxPathType.Folder:
                    this.OnCreateFolder();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(folder), $"VxPathType {folder} must be {VxPathType.Folder} or {VxPathType.File}.");
            }
        }

        /// <summary>
        /// 获取目标文件夹对象
        /// </summary>
        /// <returns></returns>
        public VxFolderInfo GetFolder() => this.OnGetFolder();

        /// <summary>
        /// 获取目标IO对象
        /// </summary>
        /// <returns></returns>
        public VxIoObject GetVxIoObject() => this.OnGetVxIoObject();

        /// <summary>
        /// 获取目标文件对象
        /// </summary>
        /// <returns></returns>
        public VxFileInfo GetFile() => this.OnGetFile();

        #endregion

        #region public properties

        /// <summary>
        /// 路径类型
        /// </summary>
        public VxPathType PathType => PathTypeInternal;

        /// <summary>
        /// 是否文件对象
        /// </summary>
        public virtual bool IsFile => this.PathType == VxPathType.File || (this.PathType == VxPathType.Unkown && !this.Exists);

        /// <summary>
        /// 是否目录对象
        /// </summary>
        public virtual bool IsFolder => this.PathType == VxPathType.Folder || (this.PathType == VxPathType.Unkown && !this.Exists);

        /// <summary>
        /// 是否根对象
        /// </summary>
        public bool IsRoot => this == this.Root;

        /// <summary>
        /// 根路径对象
        /// </summary>
        public VxPathInfo Root => this.Provider.Entry;

        /// <summary>
        /// 父对象
        /// </summary>
        public VxPathInfo Parent => Provider.GetParent(this);

        /// <summary>
        /// 提供者对象
        /// </summary>
        public VxProviderInfo Provider
        {
            get;
        }

        /// <summary>
        /// 对象是否已经存在
        /// </summary>
        public bool Exists => ExistsInternal;

        /// <summary>
        /// 当前对象完整相对路径字符串
        /// </summary>
        public string PathString
        {
            get;
        }

        /// <summary>
        /// 获取物理路径信息
        /// </summary>
        public string PhysicalPath => Provider.MapPath(this.PathString);

        /// <summary>
        /// 获取当前对象相对环境的完整路径字符串
        /// </summary>
        public string FullPathString => Provider.GetFullPathString(this);

        /// <summary>
        /// 获取对象的名称
        /// </summary>
        public string Name => this.NameInternal;

        /// <summary>
        /// 获取对象的扩展名
        /// </summary>
        public string Extension => this.ExtensionInternal;

        #endregion

        #endregion

        #region protected

        #region protected methods

        /// <summary>
        /// 获取当前对象的hash值
        /// </summary>
        /// <returns></returns>
        protected override int OnGetHashCode() => _hashCode;

        /// <summary>
        /// 诊断当前对象是否一个有效的文件夹对象
        /// </summary>
        protected virtual void AssertValidFolder()
        {
            if (!this.Exists)
            {
                throw new InvalidOperationException($"path {this.PhysicalPath} not exist.");
            }
            if (!this.IsFolder)
            {
                throw new InvalidOperationException($"path {this.PhysicalPath} is not a folder.");
            }
        }

        /// <summary>
        /// 获取所有子级路径对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        protected virtual IEnumerable<VxPathInfo> OnGetChildren(string pattern = null)
        {
            this.AssertValidFolder();
            return Provider.GetChildren(this, pattern);
        }

        /// <summary>
        /// 获取所有子级文件对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        protected virtual IEnumerable<VxPathInfo> OnGetFiles(string pattern = null)
        {
            this.AssertValidFolder();
            return Provider.GetFiles(this, pattern);
        }

        /// <summary>
        /// 获取所有子级文件夹对象
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        protected virtual IEnumerable<VxPathInfo> OnGetFolders(string pattern = null)
        {
            this.AssertValidFolder();
            return Provider.GetFolders(this, pattern);
        }

        /// <summary>
        /// 获取当前路径信息对应的文件夹对象
        /// </summary>
        /// <returns></returns>
        protected virtual VxFolderInfo OnGetFolder()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取目标IO对象
        /// </summary>
        /// <returns></returns>
        protected virtual VxIoObject OnGetVxIoObject()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取目标文件对象
        /// </summary>
        /// <returns></returns>
        protected virtual VxFileInfo OnGetFile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建目标文件夹
        /// </summary>
        protected virtual void OnCreateFolder()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建目标文件
        /// </summary>
        protected virtual void OnCreateFile()
        {
            this.OpenFile(FileMode.Create).Dispose();
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        protected virtual Stream OnOpenFile(FileMode mode)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region protected properties

        /// <summary>
        /// 路径类型
        /// </summary>
        protected virtual VxPathType PathTypeInternal
        {
            get;
            set;
        } = VxPathType.Unkown;

        /// <summary>
        /// 当前对象是否已经存在
        /// </summary>
        protected virtual bool ExistsInternal
        {
            get;
            set;
        }

        /// <summary>
        /// 获取对象的名称
        /// </summary>
        protected virtual string NameInternal
        {
            get
            {
                if (_segedPathInfo.Count == 0)
                {
                    return $"{Provider.Enviroment.SeparatorChar}";
                }
                return _segedPathInfo.Array[_segedPathInfo.Offset + _segedPathInfo.Count - 1];
            }
        }

        /// <summary>
        /// 获取对象的扩展名
        /// </summary>
        protected virtual string ExtensionInternal
        {
            get
            {
                var index = this.Name.IndexOf('.');
                if (index > 0)
                {
                    return this.Name.Substring(index);
                }
                return null;
            }
        }

        #endregion

        #endregion

        #region 接口 IVxPathInfo 显式实现

        IVxFolderInfo IVxPathInfo.GetFolder() => this.GetFolder();

        IEnumerable<IVxPathInfo> IVxPathInfo.GetChildren(string pattern) => this.GetChildren(pattern);

        IEnumerable<IVxPathInfo> IVxPathInfo.GetFiles(string pattern) => this.GetFiles(pattern);

        IEnumerable<IVxPathInfo> IVxPathInfo.GetFolders(string pattern) => this.GetFolders(pattern);

        IVxIoObject IVxPathInfo.GetVxIoObject() => this.GetVxIoObject();

        IVxFileInfo IVxPathInfo.GetFile() => this.GetFile();

        IVxPathInfo IVxPathInfo.Parent => this.Parent;

        IVxPathInfo IVxPathInfo.Root => this.Root;

        #endregion
    }
}
