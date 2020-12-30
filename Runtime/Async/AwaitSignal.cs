using System;
using System.Runtime.CompilerServices;


namespace TWizard.Core.Async
{
    public struct AwaitSignal : INotifyCompletion
    {
        private readonly Action<Action> suscription;
        private readonly Func<bool> getCompleted;

        public bool IsCompleted => getCompleted();

        public void OnCompleted(Action continuation) => suscription(continuation);

        public void GetResult() { }

        public AwaitSignal GetAwaiter() => this;
    }
}