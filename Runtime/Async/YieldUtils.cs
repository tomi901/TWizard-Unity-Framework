using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


namespace TWizard.Core.Async
{
    public static class Yield
    {
        public static YieldableTask AsYieldable(this Task task) => new YieldableTask(task);
        public static YieldableTask<T> AsYieldable<T>(this Task<T> task) => new YieldableTask<T>(task);


        public static IEnumerator Async(Func<Task> asyncFunction)
        {
            if (asyncFunction == null)
                throw new ArgumentNullException(nameof(asyncFunction));

            var awaiter = asyncFunction().GetAwaiter();
            while (!awaiter.IsCompleted)
            {
                // Debug.Log("Waiting...");
                yield return null;
            }
            awaiter.GetResult();
        }
    }

    public class YieldableTask : CustomYieldInstruction
    {
        public Task Task { get; }
        public System.Runtime.CompilerServices.TaskAwaiter Awaiter { get; }

        public override bool keepWaiting
        {
            get
            {
                if (Awaiter.IsCompleted)
                {
                    Awaiter.GetResult();
                    return false;
                }
                else return true;
            }
        }


        public YieldableTask(Task task)
        {
            Task = task ?? throw new ArgumentNullException(nameof(task));
            Awaiter = Task.GetAwaiter();
        }
    }

    public class YieldableTask<T> : CustomYieldInstruction
    {
        public Task<T> Task { get; }
        public System.Runtime.CompilerServices.TaskAwaiter<T> Awaiter { get; }

        public T Result => Task.Result;

        public override bool keepWaiting
        {
            get
            {
                if (Awaiter.IsCompleted)
                {
                    Awaiter.GetResult();
                    return false;
                }
                else return true;
            }
        }


        public YieldableTask(Task<T> task)
        {
            Task = task ?? throw new ArgumentNullException(nameof(task));
            Awaiter = Task.GetAwaiter();
        }
    }

    /// <summary>
    /// Util yieldable class to yield until <see cref="Finish"/> is called, can be reused with <see cref="Reset"/>.
    /// </summary>
    public class YieldSignal : CustomYieldInstruction
    {
        private bool wait = true;
        public override bool keepWaiting => wait;


        public void Finish() => wait = false;
        public new void Reset() => wait = true;
    }
}
