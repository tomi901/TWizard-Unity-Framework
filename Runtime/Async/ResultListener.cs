using System;
using UnityEngine;


namespace TWizard.Core
{

    /// <summary>
    /// Util yieldable class to yield a method with a <see cref="ResultCallback{T}"/>, can be used as a parameter of that type..
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    public class ResultListener<T> : CustomYieldInstruction
    {
        public struct Awaiter : System.Runtime.CompilerServices.INotifyCompletion
        {
            public readonly ResultListener<T> listener;
            public bool IsCompleted => listener.HasResult;

            public Awaiter(ResultListener<T> listener) => this.listener = listener;

            public void OnCompleted(Action continuation)
            {
                listener.OnCompleted += continuation;
            }

            public T GetResult() => listener.Result.Value;
        }

        public override bool keepWaiting => !result.HasValue;

        private Result<T>? result = null;
        private Result<T> Result => result ?? throw new NullReferenceException("Result is not set yet.");
        public bool HasResult => result.HasValue;

        public T Value => Result.Value;

        public event Action OnCompleted;

        public ResultCallback<T> Callback => SetResult;


        public void SetResult(Result<T> result)
        {
            if (HasResult)
                throw new InvalidOperationException("Already set result");

            this.result = result;
            OnCompleted?.Invoke();
        }

        public void SetResult(T result) => this.result = new Result<T>(result);
        public void SetException(Exception exception) => result = new Result<T>(exception);

        public void ThrowIfException()
        {
            var result = Result;
            if (result.HasError)
                throw result.Exception;
        }

        public new void Reset()
        {
            OnCompleted = null;
            result = null;
        }

        public Awaiter GetAwaiter() => new Awaiter();

        public static implicit operator ResultCallback<T>(ResultListener<T> listener) => listener.SetResult;
    }
}