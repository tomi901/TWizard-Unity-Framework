using System;


namespace TWizard.Core
{
    public struct Result<T>
    {
        private readonly T value;
        public T Value => IsSuccesful ? value : throw Exception;
        public Exception Exception { get; }

        public bool IsSuccesful => Exception == null;
        public bool HasError => Exception != null;


        public Result(T value)
        {
            this.value = value;
            Exception = null;
        }

        public Result(Exception exception)
        {
            value = default;
            Exception = exception;
        }


        public static implicit operator Result<T>(T value) => new Result<T>(value);
        public static implicit operator T(Result<T> result) => result.Value;
    }

    public static class ResultExtensions
    {
        public static void SetResult<T>(this Action<Result<T>> resultCallback, T value) => resultCallback(new Result<T>(value));
        public static void SetException<T>(this Action<Result<T>> resultCallback, Exception exception) => resultCallback(new Result<T>(exception));
    }
}
