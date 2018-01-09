using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chenyuan.Utilities.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UpgradeableReadLockDisposable : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rwLock"></param>
        public UpgradeableReadLockDisposable(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
        }

        void IDisposable.Dispose()
        {
            _rwLock.ExitUpgradeableReadLock();
        }

    }
}
