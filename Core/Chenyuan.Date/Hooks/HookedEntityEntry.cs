using Chenyuan.Date.Entity;

namespace Chenyuan.Data.Hooks
{
    /// <summary>
    /// 
    /// </summary>
    public class HookedEntityEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityObject Entity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityObjectState PreSaveState { get; set; }
    }
}
