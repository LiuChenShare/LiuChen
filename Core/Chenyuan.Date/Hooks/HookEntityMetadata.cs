namespace Chenyuan.Data.Hooks
{
    /// <summary>
    /// 
    /// </summary>
    public class HookEntityMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public HookEntityMetadata(EntityObjectState state)
        {
            _state = state;
        }

        private EntityObjectState _state;
        /// <summary>
        /// 
        /// </summary>
        public EntityObjectState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    HasStateChanged = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasStateChanged { get; private set; }
    }
}
