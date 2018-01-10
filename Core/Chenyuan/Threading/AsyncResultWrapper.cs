using System;
using System.Threading;

namespace Chenyuan.Threading
{
    internal sealed class SingleEntryGate
    {
        private const int NotEntered = 0;
        private const int Entered = 1;
        private int _status;
        public bool TryEnter()
        {
            int num = Interlocked.Exchange(ref _status, 1);
            return num == 0;
        }
    }

    internal sealed class SimpleAsyncResult : IAsyncResult
    {
        private readonly object _asyncState;
        private bool _completedSynchronously;
        private volatile bool _isCompleted;
        public object AsyncState
        {
            get
            {
                return _asyncState;
            }
        }
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return null;
            }
        }
        public bool CompletedSynchronously
        {
            get
            {
                return _completedSynchronously;
            }
        }
        public bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }
        }
        public SimpleAsyncResult(object asyncState)
        {
            _asyncState = asyncState;
        }
        public void MarkCompleted(bool completedSynchronously, AsyncCallback callback)
        {
            _completedSynchronously = completedSynchronously;
            _isCompleted = true;
            callback?.Invoke(this);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class AsyncResultWrapper
    {
        private static class CachedDelegates<TState>
        {
            internal static Func<AsyncCallback, object, TState, IAsyncResult> CompletedBeginInvoke = delegate (AsyncCallback asyncCallback, object asyncState, TState invokeState)
            {
                SimpleAsyncResult simpleAsyncResult = new SimpleAsyncResult(asyncState);
                simpleAsyncResult.MarkCompleted(true, asyncCallback);
                return simpleAsyncResult;
            };
        }

        private abstract class WrappedAsyncResultBase<TResult> : IAsyncResult
        {
            private const int AsyncStateNone = 0;
            private const int AsyncStateBeginUnwound = 1;
            private const int AsyncStateCallbackFired = 2;
            private int _asyncState;
            private readonly object _beginDelegateLockObj = new object();
            private readonly SingleEntryGate _endExecutedGate = new SingleEntryGate();
            private readonly SingleEntryGate _handleCallbackGate = new SingleEntryGate();
            private readonly object _tag;
            private IAsyncResult _innerAsyncResult;
            private AsyncCallback _originalCallback;
            private volatile bool _timedOut;
            private Timer _timer;
            private readonly SynchronizationContext _callbackSyncContext;
            public object AsyncState
            {
                get
                {
                    return _innerAsyncResult.AsyncState;
                }
            }
            public WaitHandle AsyncWaitHandle
            {
                get
                {
                    return null;
                }
            }
            public bool CompletedSynchronously
            {
                get;
                private set;
            }
            public bool IsCompleted
            {
                get
                {
                    return _timedOut || _innerAsyncResult.IsCompleted;
                }
            }
            protected WrappedAsyncResultBase(object tag, SynchronizationContext callbackSyncContext)
            {
                _tag = tag;
                _callbackSyncContext = callbackSyncContext;
            }
            public void Begin(AsyncCallback callback, object state, int timeout)
            {
                _originalCallback = callback;
                lock (_beginDelegateLockObj)
                {
                    _innerAsyncResult = this.CallBeginDelegate(new AsyncCallback(this.HandleAsynchronousCompletion), state);
                    int num = Interlocked.Exchange(ref _asyncState, 1);
                    this.CompletedSynchronously = (num == 2 || _innerAsyncResult.CompletedSynchronously);
                    if (!this.CompletedSynchronously && timeout > -1)
                    {
                        this.CreateTimer(timeout);
                    }
                }
                if (this.CompletedSynchronously && callback != null)
                {
                    callback(this);
                }
            }
            protected abstract IAsyncResult CallBeginDelegate(AsyncCallback callback, object callbackState);
            protected abstract TResult CallEndDelegate(IAsyncResult asyncResult);
            public static WrappedAsyncResultBase<TResult> Cast(IAsyncResult asyncResult, object tag)
            {
                if (asyncResult == null)
                {
                    throw new ArgumentNullException("asyncResult");
                }
                AsyncResultWrapper.WrappedAsyncResultBase<TResult> wrappedAsyncResultBase = asyncResult as AsyncResultWrapper.WrappedAsyncResultBase<TResult>;
                if (wrappedAsyncResultBase != null && object.Equals(wrappedAsyncResultBase._tag, tag))
                {
                    return wrappedAsyncResultBase;
                }
                throw new InvalidOperationException("invalid async result object.");
            }

            internal static void Sync(SynchronizationContext syncContext, Action action)
            {
                Sync(syncContext, delegate
                {
                    action();
                    return default(AsyncVoid);
                });
            }

            internal static T Sync<T>(SynchronizationContext syncContext, Func<T> func)
            {
                T theValue = default(T);
                Exception thrownException = null;
                syncContext.Send(delegate (object o)
                {
                    try
                    {
                        theValue = func();
                    }
                    catch (Exception e)
                    {
                        thrownException = e;
                    }
                }, null);
                if (thrownException != null)
                {
                    throw new Exception("synchronization context exception throwned.");
                }
                return theValue;
            }
            private void CallbackUsingSyncContext()
            {
                Sync(_callbackSyncContext, delegate
                {
                    _originalCallback(this);
                });
            }

            private void CreateTimer(int timeout)
            {
                _timer = new Timer(new TimerCallback(this.HandleTimeout), null, timeout, -1);
            }
            public TResult End()
            {
                if (!_endExecutedGate.TryEnter())
                {
                    throw new InvalidOperationException("async result object is be consumed.");
                }
                if (_timedOut)
                {
                    throw new TimeoutException();
                }
                this.WaitForBeginToCompleteAndDestroyTimer();
                return this.CallEndDelegate(_innerAsyncResult);
            }
            private void ExecuteAsynchronousCallback(bool timedOut)
            {
                this.WaitForBeginToCompleteAndDestroyTimer();
                if (_handleCallbackGate.TryEnter())
                {
                    _timedOut = timedOut;
                    if (_originalCallback != null)
                    {
                        if (_callbackSyncContext != null)
                        {
                            this.CallbackUsingSyncContext();
                            return;
                        }
                        _originalCallback(this);
                    }
                }
            }
            private void HandleAsynchronousCompletion(IAsyncResult asyncResult)
            {
                int num = Interlocked.Exchange(ref _asyncState, 2);
                if (num != 1)
                {
                    return;
                }
                this.ExecuteAsynchronousCallback(false);
            }
            private void HandleTimeout(object state)
            {
                this.ExecuteAsynchronousCallback(true);
            }
            private void WaitForBeginToCompleteAndDestroyTimer()
            {
                lock (_beginDelegateLockObj)
                {
                    if (_timer != null)
                    {
                        _timer.Dispose();
                    }
                    _timer = null;
                }
            }
        }
        private sealed class WrappedAsyncResult<TResult> : AsyncResultWrapper.WrappedAsyncResultBase<TResult>
        {
            private readonly Func<AsyncCallback, object, IAsyncResult> _beginDelegate;
            private readonly Func<IAsyncResult, TResult> _endDelegate;
            public WrappedAsyncResult(Func<AsyncCallback, object, IAsyncResult> beginDelegate, Func<IAsyncResult, TResult> endDelegate, object tag, SynchronizationContext callbackSyncContext) : base(tag, callbackSyncContext)
            {
                _beginDelegate = beginDelegate;
                _endDelegate = endDelegate;
            }
            protected override IAsyncResult CallBeginDelegate(AsyncCallback callback, object callbackState)
            {
                return _beginDelegate(callback, callbackState);
            }
            protected override TResult CallEndDelegate(IAsyncResult asyncResult)
            {
                return _endDelegate(asyncResult);
            }
        }
        private sealed class WrappedAsyncResult<TResult, TState> : WrappedAsyncResultBase<TResult>
        {
            private readonly Func<AsyncCallback, object, TState, IAsyncResult> _beginDelegate;
            private readonly Func<IAsyncResult, TState, TResult> _endDelegate;
            private readonly TState _state;
            public WrappedAsyncResult(Func<AsyncCallback, object, TState, IAsyncResult> beginDelegate, Func<IAsyncResult, TState, TResult> endDelegate, TState state, object tag, SynchronizationContext callbackSyncContext) : base(tag, callbackSyncContext)
            {
                _beginDelegate = beginDelegate;
                _endDelegate = endDelegate;
                _state = state;
            }
            protected override TResult CallEndDelegate(IAsyncResult asyncResult)
            {
                return _endDelegate(asyncResult, _state);
            }
            protected override IAsyncResult CallBeginDelegate(AsyncCallback callback, object callbackState)
            {
                return _beginDelegate(callback, callbackState, _state);
            }
        }
        private sealed class WrappedAsyncVoid<TState> : WrappedAsyncResultBase<AsyncVoid>
        {
            private readonly Func<AsyncCallback, object, TState, IAsyncResult> _beginDelegate;
            private readonly Action<IAsyncResult, TState> _endDelegate;
            private readonly TState _state;
            public WrappedAsyncVoid(Func<AsyncCallback, object, TState, IAsyncResult> beginDelegate, Action<IAsyncResult, TState> endDelegate, TState state, object tag, SynchronizationContext callbackSyncContext) : base(tag, callbackSyncContext)
            {
                _beginDelegate = beginDelegate;
                _endDelegate = endDelegate;
                _state = state;
            }
            protected override AsyncVoid CallEndDelegate(IAsyncResult asyncResult)
            {
                _endDelegate(asyncResult, _state);
                return default(AsyncVoid);
            }
            protected override IAsyncResult CallBeginDelegate(AsyncCallback callback, object callbackState)
            {
                return _beginDelegate(callback, callbackState, _state);
            }
        }
        private static readonly Func<IAsyncResult, Action, AsyncVoid> _voidEndInvoke = delegate (IAsyncResult asyncResult, Action action)
        {
            action();
            return default(AsyncVoid);
        };
        public static IAsyncResult Begin<TResult>(AsyncCallback callback, object state, Func<AsyncCallback, object, IAsyncResult> beginDelegate, Func<IAsyncResult, TResult> endDelegate, object tag = null, int timeout = -1)
        {
            WrappedAsyncResult<TResult> wrappedAsyncResult = new WrappedAsyncResult<TResult>(beginDelegate, endDelegate, tag, null);
            wrappedAsyncResult.Begin(callback, state, timeout);
            return wrappedAsyncResult;
        }
        public static IAsyncResult Begin<TResult, TState>(AsyncCallback callback, object callbackState, Func<AsyncCallback, object, TState, IAsyncResult> beginDelegate, Func<IAsyncResult, TState, TResult> endDelegate, TState invokeState, object tag = null, int timeout = -1, SynchronizationContext callbackSyncContext = null)
        {
            WrappedAsyncResult<TResult, TState> wrappedAsyncResult = new WrappedAsyncResult<TResult, TState>(beginDelegate, endDelegate, invokeState, tag, callbackSyncContext);
            wrappedAsyncResult.Begin(callback, callbackState, timeout);
            return wrappedAsyncResult;
        }
        public static IAsyncResult Begin<TState>(AsyncCallback callback, object callbackState, Func<AsyncCallback, object, TState, IAsyncResult> beginDelegate, Action<IAsyncResult, TState> endDelegate, TState invokeState, object tag = null, int timeout = -1, SynchronizationContext callbackSyncContext = null)
        {
            WrappedAsyncVoid<TState> wrappedAsyncVoid = new WrappedAsyncVoid<TState>(beginDelegate, endDelegate, invokeState, tag, callbackSyncContext);
            wrappedAsyncVoid.Begin(callback, callbackState, timeout);
            return wrappedAsyncVoid;
        }
        public static IAsyncResult BeginSynchronous<TResult, TState>(AsyncCallback callback, object callbackState, Func<IAsyncResult, TState, TResult> func, TState funcState, object tag)
        {
            Func<AsyncCallback, object, TState, IAsyncResult> completedBeginInvoke = CachedDelegates<TState>.CompletedBeginInvoke;
            WrappedAsyncResult<TResult, TState> wrappedAsyncResult = new WrappedAsyncResult<TResult, TState>(completedBeginInvoke, func, funcState, tag, null);
            wrappedAsyncResult.Begin(callback, callbackState, -1);
            return wrappedAsyncResult;
        }
        public static IAsyncResult BeginSynchronous(AsyncCallback callback, object state, Action action, object tag)
        {
            return BeginSynchronous(callback, state, _voidEndInvoke, action, tag);
        }
        public static TResult End<TResult>(IAsyncResult asyncResult)
        {
            return End<TResult>(asyncResult, null);
        }
        public static TResult End<TResult>(IAsyncResult asyncResult, object tag)
        {
            return WrappedAsyncResultBase<TResult>.Cast(asyncResult, tag).End();
        }
        public static void End(IAsyncResult asyncResult)
        {
            End(asyncResult, null);
        }
        public static void End(IAsyncResult asyncResult, object tag)
        {
            End<AsyncVoid>(asyncResult, tag);
        }
    }
}
