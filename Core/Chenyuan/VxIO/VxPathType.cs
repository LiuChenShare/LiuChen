namespace Chenyuan.VxIO
{
    /// <summary>
    /// 虚拟路径类型枚举定义
    /// </summary>
    public enum VxPathType
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        /// <remarks>主要指未确定的，如给定一个字符串，但目标对象本身不存在，此时则为未知</remarks>
        Unkown,
        /// <summary>
        /// 文件对象
        /// </summary>
        File,
        /// <summary>
        /// 目录对象
        /// </summary>
        Folder,
    }
}
