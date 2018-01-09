using Chenyuan.Components;
using Chenyuan.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan
{
    /// <summary>
	/// Represents a common helper
	/// </summary>
	public partial class CommonHelper
    {
        static ICommonHelper s_commonHelper;
        /// <summary>
        /// 
        /// </summary>
        public static ICommonHelper Default
        {
            get
            {
                if (s_commonHelper == null)
                {
                    LoadCommonHelper();
                }
                return s_commonHelper;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commonHelper"></param>
        public static void SetCommonHelper(ICommonHelper commonHelper)
        {
            s_commonHelper = commonHelper;
        }

        private static void LoadCommonHelper()
        {
            ITypeFinder typeFinder = EngineContext.Current.Resolve<ITypeFinder>();
            if (typeFinder == null)
                return;
            var types = typeFinder.FindClassesOfType<ICommonHelper>(null, true);
            foreach (Type type in types)
            {
                try
                {
                    s_commonHelper = Activator.CreateInstance(type) as ICommonHelper;
                    return;
                }
                catch { };
            }
        }
    }
}
