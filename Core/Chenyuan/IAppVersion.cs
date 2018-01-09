using System;
using System.Collections.Generic;

namespace Chenyuan
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppVersion
    {
        /// <summary>
        /// Gets the app version
        /// </summary>
        string CurrentVersion
        {
            get;
        }

        /// <summary>
        /// Gets the app full version
        /// </summary>
        string CurrentFullVersion
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        Version FullVersion
        {
            get;
        }

        /// <summary>
        /// Gets a list of Lifenxiang.Net versions in which breaking changes occured,
        /// which could lead to plugins malfunctioning.
        /// </summary>
        /// <remarks>
        /// A plugin's <c>MinAppVersion</c> is checked against this list to assume
        /// it's compatibility with the current app version.
        /// </remarks>
        IEnumerable<Version> BreakingChangesHistory
        {
            get;
        }
    }
}
