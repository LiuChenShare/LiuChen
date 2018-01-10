using Chenyuan.Date.Entity;

namespace Chenyuan.Data.Hooks
{
    /// <summary>
    /// Implements a hook that will run before an entity gets deleted from the database.
    /// </summary>
    public abstract class PreDeleteHook<TEntity> : PreActionHook<TEntity>
        where TEntity : EntityObject
    {
        /// <summary>
        /// Returns <see cref="EntityObjectState.Deleted"/> as the hookstate to listen for.
        /// </summary>
        public sealed override EntityObjectState HookStates
        {
            get { return EntityObjectState.Deleted; }
        }
    }
}
