﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chenyuan.Utilities.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public static class LockExtensions
    {

        /// <summary>
        /// Acquires a disposable reader lock that can be used with a using statement.
        /// </summary>
        [DebuggerStepThrough]
        public static IDisposable GetReadLock(this ReaderWriterLockSlim rwLock)
        {
            return rwLock.GetReadLock(-1);
        }

        /// <summary>
        /// Acquires a disposable reader lock that can be used with a using statement.
        /// </summary>
        /// <param name="rwLock"></param>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait, or -1 to wait indefinitely.
        /// </param>
        [DebuggerStepThrough]
        public static IDisposable GetReadLock(this ReaderWriterLockSlim rwLock, int millisecondsTimeout)
        {
            bool acquire = rwLock.IsReadLockHeld == false ||
                           rwLock.RecursionPolicy == LockRecursionPolicy.SupportsRecursion;

            if (acquire)
            {
                if (rwLock.TryEnterReadLock(millisecondsTimeout))
                {
                    return new ReadLockDisposable(rwLock);
                }
            }

            return ActionDisposable.Empty;
        }

        /// <summary>
        /// Acquires a disposable and upgradeable reader lock that can be used with a using statement.
        /// </summary>
        /// <param name="rwLock"></param>
        [DebuggerStepThrough]
        public static IDisposable GetUpgradeableReadLock(this ReaderWriterLockSlim rwLock)
        {
            return rwLock.GetUpgradeableReadLock(-1);
        }

        /// <summary>
        /// Acquires a disposable and upgradeable reader lock that can be used with a using statement.
        /// </summary>
        /// <param name="rwLock"></param>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait, or -1 to wait indefinitely.
        /// </param>
        [DebuggerStepThrough]
        public static IDisposable GetUpgradeableReadLock(this ReaderWriterLockSlim rwLock, int millisecondsTimeout)
        {
            bool acquire = rwLock.IsUpgradeableReadLockHeld == false ||
                           rwLock.RecursionPolicy == LockRecursionPolicy.SupportsRecursion;

            if (acquire)
            {
                if (rwLock.TryEnterUpgradeableReadLock(millisecondsTimeout))
                {
                    return new UpgradeableReadLockDisposable(rwLock);
                }
            }

            return ActionDisposable.Empty;
        }

        /// <summary>
        /// Acquires a disposable writer lock that can be used with a using statement.
        /// </summary>
        /// <param name="rwLock"></param>
        [DebuggerStepThrough]
        public static IDisposable GetWriteLock(this ReaderWriterLockSlim rwLock)
        {
            return rwLock.GetWriteLock(-1);
        }

        /// <summary>
        /// Tries to enter a disposable write lock that can be used with a using statement.
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait, or -1 to wait indefinitely.
        /// </param>
        /// <param name="rwLock"></param>
        [DebuggerStepThrough]
        public static IDisposable GetWriteLock(this ReaderWriterLockSlim rwLock, int millisecondsTimeout)
        {
            bool acquire = rwLock.IsWriteLockHeld == false ||
                           rwLock.RecursionPolicy == LockRecursionPolicy.SupportsRecursion;

            if (acquire)
            {
                if (rwLock.TryEnterWriteLock(millisecondsTimeout))
                {
                    return new WriteLockDisposable(rwLock);
                }
            }

            return ActionDisposable.Empty;
        }

    }
}
