using System;

namespace Chenyuan.VxIO
{
    /// <summary>
    /// 虚拟IO对象基类定义
    /// </summary>
    [Serializable]
    public abstract class VxIoObject : VxObjectInfo, IVxIoObject
    {
        #region 构造

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="virtualPath"></param>
        protected VxIoObject(IVxPathInfo virtualPath)
        {
            if (virtualPath == null)
            {
                throw new ArgumentNullException(nameof(virtualPath));
            }
            this.VxPathObject = this.VerifyVxPathObject(virtualPath);
            this.OnInitialize(this.VxPathObject);
        }

        /// <summary>
        /// 验证路径对象是否一个有效的目标对象
        /// </summary>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        protected abstract IVxPathInfo VerifyVxPathObject(IVxPathInfo pathInfo);

        /// <summary>
        /// 执行初始化
        /// </summary>
        /// <param name="pathInfo"></param>
        protected virtual void OnInitialize(IVxPathInfo pathInfo)
        {
        }

        #endregion

        /// <summary>
        /// 获取对象名称
        /// </summary>
        public virtual string Name => VxPathObject.Name;

        /// <summary>
        /// 获取对象虚拟路径
        /// </summary>
        public string VxPath => VxPathObject.PathString;

        /// <summary>
        /// 获取目标虚拟路径对象
        /// </summary>
        protected IVxPathInfo VxPathObject
        {
            get;
        }

        /// <summary>
        /// 是否文件夹
        /// </summary>
        public abstract bool IsFolder
        {
            get;
        }

        /// <summary>
        /// 相对 provider 的完整路径名称
        /// </summary>
        public string Fullname => VxPathObject.FullPathString;

        /// <summary>
        /// 完整物理路径
        /// </summary>
        public string PhysicalPath => VxPathObject.PhysicalPath;

        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension => VxPathObject.Extension;

        /// <summary>
        /// 获取hash值
        /// </summary>
        /// <returns></returns>
        protected override int OnGetHashCode() => this.VxPathObject.GetHashCode();
    }
}
