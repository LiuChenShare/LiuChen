﻿namespace Chenyuan.Data.Hooks
{
    /// <summary>
    /// A hook that is executed before an action.
    /// </summary>
    public interface IPreActionHook : IHook
    {
        /// <summary>
        /// Gets a value indicating whether the hook is only used after successful validation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if requires validation; otherwise, <c>false</c>.
        /// </value>
        bool RequiresValidation { get; }

        /// <summary>
        /// Gets the entity state to listen for.
        /// </summary>
        /// <value>
        /// The hook states.
        /// </value>
        EntityObjectState HookStates { get; }
    }
}
