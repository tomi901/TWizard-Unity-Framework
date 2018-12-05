using System;
using System.Collections;
using UnityEngine;

using Object = UnityEngine.Object;


namespace Tomi.Utils
{
    public static partial class UnityUtils
    {

        public static void LogWithContext(this Object obj, string message)
        {
            Debug.Log(message, obj);
        }

        public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time)
        {
            return monoBehaviour.StartCoroutine(WaitForAction(action, time));
        }

        public static Coroutine InvokeRepeating(this MonoBehaviour monoBehaviour, Action action, float time, 
            float repeatTime)
        {
            return monoBehaviour.StartCoroutine(WaitAndRepeatAction(action, time, repeatTime));
        }

        private static IEnumerator WaitForAction(Action action, float time)
        {
            if (action == null) yield break;

            yield return new WaitForSeconds(time);
            action.Invoke();
        }

        private static IEnumerator WaitAndRepeatAction(Action action, float time, float repeatTime)
        {
            if (action == null) yield break;

            yield return new WaitForSeconds(time);

            WaitForSeconds repeatWait = new WaitForSeconds(repeatTime);
            while (true)
            {
                action.Invoke();
                yield return repeatWait;
            }
        }

    }
}
