using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;


namespace TWizard.Core.Async
{
    public static class AsyncCoroutines
    {
        public static Task<T> AsTask<T>(this T instruction) where T : YieldInstruction => UnityEvents.Instance.AwaitAsCoroutine(instruction);


        public static Task<object> AsTask(this IEnumerator enumerator) => UnityEvents.Instance.AwaitAsCoroutine(enumerator);
        /// <summary>
        /// Awaits an <see cref="IEnumerator"/> through a <see cref="Coroutine"/>, it returns the last <see cref="object"/> emitted by the enumerator.
        /// </summary>
        /// <param name="enumerator">The enumerator to await with coroutines.</param>
        /// <returns>The last emitted <see cref="object"/> by the yield.</returns>
        public static TaskAwaiter<object> GetAwaiter(this IEnumerator enumerator) => enumerator.AsTask().GetAwaiter();
    }
}
