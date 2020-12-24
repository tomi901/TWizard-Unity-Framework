using System;
using System.Threading.Tasks;
using UnityEngine;


namespace TWizard.Core.Async
{
    public class YieldableTask : CustomYieldInstruction
    {
        public Task Task { get; }

        public override bool keepWaiting => Task.ShouldKeepWaiting();


        public YieldableTask(Task task) => Task = task ?? throw new ArgumentNullException(nameof(task));
    }

    public class YieldableTask<T> : CustomYieldInstruction
    {
        public Task<T> Task { get; }
        public T Result => Task.Result;

        public override bool keepWaiting => Task.ShouldKeepWaiting();


        public YieldableTask(Task<T> task) => Task = task ?? throw new ArgumentNullException(nameof(task));
    }


    public class YieldSignal : CustomYieldInstruction
    {
        private bool wait = true;
        public override bool keepWaiting => wait;


        public void Finish() => wait = false;
        public new void Reset() => wait = true;
    }


    public static class Yield
    {
        /// <summary>
        /// Creates a new yieldable signal, complete it with <see cref="YieldSignal.Finish"/>.
        /// </summary>
        public static YieldSignal Signal => new YieldSignal();


        public static bool ShouldKeepWaiting(this Task task) => !task.IsCompleted || !task.IsFaulted || !task.IsCanceled;


        public static YieldableTask AsYieldable(this Task task) => new YieldableTask(task);
        public static YieldableTask<T> AsYieldable<T>(this Task<T> task) => new YieldableTask<T>(task);
    }
}
