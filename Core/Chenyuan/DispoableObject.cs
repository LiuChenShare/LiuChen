using System;
using System.Threading;
using Chenyuan.Utilities.Threading;

namespace Chenyuan
{
    /// <summary>
    /// 可处置对象类定义
    /// </summary>
    [Serializable]
    public abstract class DispoableObject : IDisposable
    {
        private readonly ReaderWriterLockSlim _disposeLocker = new ReaderWriterLockSlim();
        private bool _disposed = false;
        /// <summary>
        /// 获取当前对象的处置状态
        /// </summary>
        protected bool Disposed => _disposed;

        /// <summary>
        /// 执行处置操作
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// 执行处置操作
        /// </summary>
        public void Dispose()
        {
            using (new ReadLockDisposable(_disposeLocker))
            {
                if (_disposed)
                {
                    return;
                }
                _disposed = true;
            }
            this.Dispose(!_disposed);
        }
    }
}
