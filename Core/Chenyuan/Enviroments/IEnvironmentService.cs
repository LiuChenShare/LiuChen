namespace Chenyuan.Enviroments
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEnvironmentService
    {
        /// <summary>
        /// 是否附加到了应用
        /// </summary>
        bool IsHosted
        {
            get;
        }

        /// <summary>
        /// 路径映射
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string MapPath(string path);
    }
}
