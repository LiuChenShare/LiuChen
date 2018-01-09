using Chenyuan.Components;
using Chenyuan.Enviroments;

namespace Chenyuan
{
    /// <summary>
    /// 
    /// </summary>
    public static class Enviroment
    {
        private static readonly IEnvironmentService s_defaultService = new DefaultEnvironmentService();
        private static IEnvironmentService s_service;
        /// <summary>
        /// 获取当前服务
        /// </summary>
        public static IEnvironmentService Service
        {
            get
            {
                if (s_service == null)
                {
                    s_service = ComponentManager.ResolveOptional<IEnvironmentService>();
                }
                if (s_service == null)
                {
                    return s_defaultService;
                }
                return s_service;
            }
            set
            {
                s_service = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsHosted => Service.IsHosted;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MapPath(string path) => Service.MapPath(path);
    }
}
