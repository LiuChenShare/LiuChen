using System;
using System.Threading;

namespace Chenyuan.Threading
{
    public static class SynchronizationContextUtil
	{
		public static SynchronizationContext GetSynchronizationContext()
		{
			return SynchronizationContext.Current ?? new SynchronizationContext();
		}
		public static T Sync<T>(this SynchronizationContext syncContext, Func<T> func)
		{
			T theValue = default(T);
			Exception thrownException = null;
			syncContext.Send(delegate(object o)
			{
				try
				{
					theValue = func();
				}
				catch (Exception exception)
				{
                    thrownException = exception;
				}
			}, null);
			if (thrownException != null)
			{
				//throw Error.SynchronizationContextUtil_ExceptionThrown(thrownException);
			}
			return theValue;
		}
		public static void Sync(this SynchronizationContext syncContext, Action action)
		{
			syncContext.Sync(delegate
			{
				action();
				return default(AsyncVoid);
			});
		}
	}
}
