
namespace Chenyuan.Data.Hooks
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHook
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="metadata"></param>
        void HookObject(object entity, HookEntityMetadata metadata);
    }
}
