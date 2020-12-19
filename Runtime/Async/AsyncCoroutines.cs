using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;


namespace TWizard.Core.Async
{
    public static class AsyncCoroutines
    {
        public static Task AsTask(this Coroutine coroutine) => UnityEvents.Instance.AwaitAsCoroutine(coroutine);
        public static TaskAwaiter GetAwaiter(this Coroutine coroutine) => UnityEvents.Instance.AwaitAsCoroutine(coroutine).GetAwaiter();


        public static Task AsTask(this IEnumerator enumerator) => UnityEvents.Instance.AwaitAsCoroutine(enumerator);

        public static TaskAwaiter GetAwaiter(this IEnumerator enumerator) => UnityEvents.Instance.AwaitAsCoroutine(enumerator).GetAwaiter();
    }
}
