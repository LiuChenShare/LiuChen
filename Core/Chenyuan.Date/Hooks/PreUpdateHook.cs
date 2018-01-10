using Chenyuan.Date.Entity;

namespace Chenyuan.Data.Hooks
{
    /// <summary>
    /// Implements a hook that will run before an entity gets updated in the database.
    /// </summary>
    public abstract class PreUpdateHook<TEntity> : PreActionHook<TEntity>
        where TEntity : EntityObject
    {
        /// <summary>
        /// Returns <see cref="EntityObjectState.Modified"/> as the hookstate to listen for.
        /// </summary>
        public sealed override EntityObjectState HookStates
        {
            get { return EntityObjectState.Modified; }
        }
    }
}
