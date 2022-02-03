using System;
using System.Threading.Tasks;

namespace CS.Edu.Core.Extensions
{
    public static class Tasks
    {
        public static async Task<T> WithTimeout<T>(this Task<T> task, int time)
        {
            var delayTask = Task.Delay(time);
            var firstToFinish = await Task.WhenAny(task, delayTask);

            if (firstToFinish == delayTask)
            {
                throw new TimeoutException();
            }

            return await task;
        }

        public static async Task<T> WithTimeout<T>(this Task<T> task, int time, Action<Task<T>> onTimedOut)
        {
            var delayTask = Task.Delay(time);
            var firstToFinish = await Task.WhenAny(task, delayTask);

            if (firstToFinish == delayTask)
            {
                task.ContinueWith(onTimedOut);
                throw new TimeoutException();
            }

            return await task;
        }

        public static async Task<T> OnFaulted<T>(this Task<T> task, Action<Task<T>> onFaulted)
        {
            try
            {
                return await task;
            }
            catch (Exception e)
            {
                onFaulted(task);
                throw;
            }
        }

        public static async Task<T> OnSucceeded<T>(this Task<T> task, Action<Task<T>> onSucceeded)
        {
            var result = await task;
            onSucceeded(task);

            return result;
        }
    }
}