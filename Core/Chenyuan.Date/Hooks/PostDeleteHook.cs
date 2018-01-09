using Chenyuan.Data.Entity;

namespace Chenyuan.Data.Hooks
{
    /// <summary>
    /// Implements a hook that will run after an entity gets deleted from the database.
    /// </summary>
    public abstract class PostDeleteHook<TEntity> : PostActionHook<TEntity>
        where TEntity : EntityObject
    {
        /// <summary>
        /// 
        /// </summary>
        public sealed override EntityObjectState HookStates
        {
            get { return EntityObjectState.Deleted; }
        }
    }
}
