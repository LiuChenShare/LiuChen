using Chenyuan.Data.Entity;

namespace Chenyuan.Data.Hooks
{
    /// <summary>
    /// Implements a hook that will run before an entity gets inserted into the database.
    /// </summary>
    public abstract class PreInsertHook<TEntity> : PreActionHook<TEntity>
        where TEntity : EntityObject
    {
        /// <summary>
        /// Returns <see cref="EntityObjectState.Added"/> as the hookstate to listen for.
        /// </summary>
        public sealed override EntityObjectState HookStates
        {
            get { return EntityObjectState.Added; }
        }
    }
}
