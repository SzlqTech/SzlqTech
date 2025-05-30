using System.Collections.Concurrent;

namespace SzlqTech.Common.MultiThreads
{
    public class SemaphoreQueue
    {
        private readonly SemaphoreSlim semaphore;

        private readonly ConcurrentQueue<TaskCompletionSource<bool>> queue = new ConcurrentQueue<TaskCompletionSource<bool>>();

        public SemaphoreQueue(int initialCount)
        {
            semaphore = new SemaphoreSlim(initialCount);
        }

        public SemaphoreQueue(int initialCount, int maxCount)
        {
            semaphore = new SemaphoreSlim(initialCount, maxCount);
        }

        public void Wait()
        {
            if (queue.Count > 0)
            {
                WaitAsync().Wait();
            }
        }

        public Task WaitAsync()
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            queue.Enqueue(taskCompletionSource);
            semaphore.WaitAsync().ContinueWith(delegate
            {
                if (queue.TryDequeue(out TaskCompletionSource<bool> result))
                {
                    result.SetResult(result: true);
                }
            });
            return taskCompletionSource.Task;
        }

        public void Release()
        {
            semaphore.Release();
        }
    }
}
