namespace Chenyuan.VxIO
{
    /// <summary>
    /// 虚拟IO对象接口定义
    /// </summary>
    public interface IVxIoObject : IVxObjectInfo
    {
        /// <summary>
        /// 对象名
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 对象相对provider的完整名
        /// </summary>
        string Fullname
        {
            get;
        }

        /// <summary>
        /// 相对路径
        /// </summary>
        string VxPath
        {
            get;
        }

        /// <summary>
        /// 获取扩展名
        /// </summary>
        string Extension
        {
            get;
        }

        /// <summary>
        /// 获取完整物理路径
        /// </summary>
        string PhysicalPath
        {
            get;
        }

        /// <summary>
        /// 是否文件夹对象
        /// </summary>
        bool IsFolder
        {
            get;
        }
    }
}
