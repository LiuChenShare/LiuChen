using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Chenyuan.Caching.Defaults
{
    internal class DisposableGCHandleRef<T> : IDisposable where T : class, IDisposable
    {
        private GCHandle _handle;
        public T Target
        {
            [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
            get
            {
                return (T)((object)_handle.Target);
            }
        }
        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        public DisposableGCHandleRef(T t)
        {
            _handle = GCHandle.Alloc(t);
        }
        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        public void Dispose()
        {
            this.Target.Dispose();
            if (_handle.IsAllocated)
            {
                _handle.Free();
            }
        }
    }
}