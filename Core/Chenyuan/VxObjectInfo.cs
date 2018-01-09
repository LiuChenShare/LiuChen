using System;
using System.Collections.Generic;

namespace Chenyuan
{
    /// <summary>
    /// 虚拟对象类实现定义
    /// </summary>
    [Serializable]
    public abstract class VxObjectInfo : MarshalByRefObject, IVxObjectInfo
    {
        /// <summary>
        /// 对象相等判断
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks>本方法调用内置 <see cref="OnCompareTo(IVxObjectInfo)"/> 实现具体的比较</remarks>
        public sealed override bool Equals(object obj)
        {
            if (obj is IVxObjectInfo)
            {
                return this.OnCompareTo(obj as IVxObjectInfo) == 0;
            }
            return false;
        }

        /// <summary>
        /// 获取对象的哈希值
        /// </summary>
        /// <returns></returns>
        /// <remarks>本方法调用内置 <see cref="OnGetHashCode()"/> 实现目标hash值的获取</remarks>
        public sealed override int GetHashCode() => this.OnGetHashCode();

        /// <summary>
        /// 实现两个对象的比较操作
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks>该函数的比较默认比较两个对象的hash值是否一致</remarks>
        protected virtual int OnCompareTo(IVxObjectInfo other)
        {
            int hash1 = this.GetHashCode();
            int hash2 = other.GetHashCode();
            if (hash1 < hash2)
            {
                return -1;
            }
            if (hash1 > hash2)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 获取当前对象的hash值
        /// </summary>
        /// <returns></returns>
        /// <remarks>该对象默认获取系统默认的hash值计算方案</remarks>
        protected virtual int OnGetHashCode()
        {
            return base.GetHashCode();
        }

        #region 接口实现

        int IComparer<IVxObjectInfo>.Compare(IVxObjectInfo x, IVxObjectInfo y)
        {
            if (x == null || y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            return (x as IComparable<IVxObjectInfo>).CompareTo(y);
        }

        int IComparable<IVxObjectInfo>.CompareTo(IVxObjectInfo other)
        {
            if (other == null)
            {
                return 1;
            }
            return this.OnCompareTo(other);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj is IVxObjectInfo)
            {
                return (this as IComparable<IVxObjectInfo>).CompareTo(obj as IVxObjectInfo);
            }
            return 1;
        }

        #endregion
    }
}
