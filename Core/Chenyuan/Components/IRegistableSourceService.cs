namespace Chenyuan.Components
{
    /// <summary>
    /// 可注册源加载服务
    /// </summary>
    public interface IRegistableSourceService
    {
        ///// <summary>
        ///// 加载可注册源对象
        ///// </summary>
        ///// <returns>目标注册源实例对象</returns>
        //IRegistableSource LoadSource();

        /// <summary>
        /// 加载可注册源对象
        /// </summary>
        /// <typeparam name="TRegistableSource">可注册源对象类型</typeparam>
        /// <returns></returns>
        TRegistableSource LoadSource<TRegistableSource>()
            where TRegistableSource : IRegistableSource;
    }
}
