using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chenyuan.Utilities.Threading
{
    /// <summary>
    /// /
    /// </summary>
    public sealed class WriteLockDisposable : IDisposable
    {
        // Fields
        private readonly ReaderWriterLockSlim _rwLock;

        // Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rwLock"></param>
        public WriteLockDisposable(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
        }

        void IDisposable.Dispose()
        {
            _rwLock.ExitWriteLock();
        }
    }
}
