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
    public sealed class ReadLockDisposable : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rwLock"></param>
        public ReadLockDisposable(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
        }

        /// <summary>
        /// 
        /// </summary>
        void IDisposable.Dispose()
        {
            _rwLock.ExitReadLock();
        }


    }
}
