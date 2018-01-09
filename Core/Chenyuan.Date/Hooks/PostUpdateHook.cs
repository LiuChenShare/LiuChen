using Chenyuan.Data.Entity;

namespace Chenyuan.Data.Hooks
{
    /// <summary>
    /// Implements a hook that will run after an entity gets updated in the database.
    /// </summary>
    public abstract class PostUpdateHook<TEntity> : PostActionHook<TEntity>
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
