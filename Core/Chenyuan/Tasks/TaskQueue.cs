using System;
using System.Threading.Tasks;

namespace Chenyuan.Tasks
{
    /// <summary>
    /// Task队列类
    /// </summary>
	public sealed class TaskQueue
	{
		private readonly object _lockObj = new object();
		private Task _lastQueuedTask = Task.FromResult<int>(0);

        /// <summary>
        /// 入队操作
        /// </summary>
        /// <param name="taskFunc"></param>
        /// <returns></returns>
		public Task Enqueue(Func<Task> taskFunc)
		{
			Task result;
			lock (_lockObj)
			{
				Task task = _lastQueuedTask.ContinueWith<Task>((Task _) => taskFunc(), TaskContinuationOptions.OnlyOnRanToCompletion).Unwrap();
				_lastQueuedTask = task;
				result = task;
			}
			return result;
		}
	}
}
