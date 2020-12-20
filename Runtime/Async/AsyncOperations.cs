using System.Runtime.CompilerServices;
using UnityEngine;


namespace TWizard.Core.Async
{
    public static class AsyncOperations
    {
        #region Base
        public struct BaseAwaiter : INotifyCompletion
        {
            public AsyncOperation operation;

            public bool IsCompleted => operation.isDone;

            public BaseAwaiter(AsyncOperation operation)
            {
                this.operation = operation;
            }

            public void OnCompleted(System.Action continuation)
            {
                operation.completed += (op) => continuation();
            }

            public AsyncOperation GetResult() => operation;
        }

        public static BaseAwaiter GetAwaiter(this AsyncOperation operation) => new BaseAwaiter(operation);
        #endregion

        #region Resource
        public struct ResourceAwaiter : INotifyCompletion
        {
            public ResourceRequest operation;

            public bool IsCompleted => operation.isDone;

            public ResourceAwaiter(ResourceRequest operation)
            {
                this.operation = operation;
            }

            public void OnCompleted(System.Action continuation)
            {
                operation.completed += (op) => continuation();
            }

            public Object GetResult() => operation.asset;
        }

        public static ResourceAwaiter GetAwaiter(this ResourceRequest operation) => new ResourceAwaiter(operation);
        #endregion
    }
}