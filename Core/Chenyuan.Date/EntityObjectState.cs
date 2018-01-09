using System;

namespace Chenyuan.Data
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum EntityObjectState
    {
        /// <summary>
        /// 
        /// </summary>
        Detached = 1,
        /// <summary>
        /// 
        /// </summary>
        Unchanged = 2,
        /// <summary>
        /// 
        /// </summary>
        Added = 4,
        /// <summary>
        /// 
        /// </summary>
        Deleted = 8,
        /// <summary>
        /// 
        /// </summary>
        Modified = 16
    }
}
