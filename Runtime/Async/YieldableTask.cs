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

    /// <summary>
    /// Util yieldable class to yield a method with a <see cref="ResultCallback{T}"/>, can be used as a parameter of that type..
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    public class ResultListener<T> : CustomYieldInstruction
    {
        public override bool keepWaiting => !result.HasValue;

        private Result<T>? result = null;
        private Result<T> Result => result ?? throw new NullReferenceException("Result is not set yet.");
        public bool HasResult => result.HasValue;

        public T Value => Result.Value;


        public void SetResult(Result<T> result) => this.result = result;
        public void SetResult(T result) => this.result = new Result<T>(result);
        public void SetException(Exception exception) => result = new Result<T>(exception);

        public void ThrowIfException()
        {
            var result = Result;
            if (result.HasError)
                throw result.Exception;
        }


        public static implicit operator ResultCallback<T>(ResultListener<T> listener) => listener.SetResult;
    }


    public static class Yield
    {
        public static bool ShouldKeepWaiting(this Task task) => !task.IsCompleted || !task.IsFaulted || !task.IsCanceled;


        public static YieldableTask AsYieldable(this Task task) => new YieldableTask(task);
        public static YieldableTask<T> AsYieldable<T>(this Task<T> task) => new YieldableTask<T>(task);
    }
}
