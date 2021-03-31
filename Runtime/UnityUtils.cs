using System;
using System.Collections;
using System.Threading;
using UnityEngine;

using Object = UnityEngine.Object;


namespace TWizard.Core
{
    public static partial class UnityUtils
    {
        /// <summary>
        /// Replaces <see cref="MonoBehaviour.Invoke(string, float)"/> and avoid calling a method by its name.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour that will own the coroutine.</param>
        /// <param name="action">The action to invoke.</param>
        /// <param name="time">How much time to wait.</param>
        /// <returns>Cancellable <see cref="Coroutine"/>.</returns>
        public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time, CancellationToken cancellationToken = default)
        {
            var coroutine = monoBehaviour.StartCoroutine(Routine());
            cancellationToken.Register(() => monoBehaviour.StopCoroutine(coroutine), true);
            return coroutine;

            IEnumerator Routine()
            {
                if (action == null) yield break;

                yield return new WaitForSeconds(time);
                action.Invoke();
            }
        }

        /// <summary>
        /// Replaces <see cref="MonoBehaviour.InvokeRepeating(string, float, float)"/> and avoid calling a method by its name.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour that will own the coroutine.</param>
        /// <param name="action">The action to invoke.</param>
        /// <param name="time">How much time to wait.</param>
        /// <param name="repeatTime"></param>
        /// <returns>Cancellable <see cref="Coroutine"/>.</returns>
        public static Coroutine InvokeRepeating(this MonoBehaviour monoBehaviour, Action action, float time, 
            float repeatTime, CancellationToken cancellationToken = default)
        {
            var coroutine = monoBehaviour.StartCoroutine(Routine());
            cancellationToken.Register(() => monoBehaviour.StopCoroutine(coroutine), true);
            return coroutine;

            IEnumerator Routine()
            {
                if (action == null)
                    yield break;

                yield return new WaitForSeconds(time);

                WaitForSeconds repeatWait = new WaitForSeconds(repeatTime);
                while (true)
                {
                    action.Invoke();
                    yield return repeatWait;
                }
            }
        }

        /// <summary>
        /// Makes an Unity <see cref="Object"/> operable with null operators like "foo?.bar();" or "foo ?? bar".
        /// Makes this by transforming it to boolean and returning null when falsey.
        /// </summary>
        /// <typeparam name="T">The <see cref="Object"/> type.</typeparam>
        /// <param name="obj">The unity object.</param>
        /// <returns>The same object, null when falsey.</returns>
        public static T NullOperable<T>(this T obj) where T : Object => obj ? obj : null;
    }
}
