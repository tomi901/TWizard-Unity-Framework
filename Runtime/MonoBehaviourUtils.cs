using System;
using System.Collections;
using UnityEngine;


namespace TWizard.Core
{
    public static partial class UnityUtils
    {
        public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time)
        {
            return monoBehaviour.StartCoroutine(Routine());

            IEnumerator Routine()
            {
                if (action == null) yield break;

                yield return new WaitForSeconds(time);
                action.Invoke();
            }
        }

        public static Coroutine InvokeRepeating(this MonoBehaviour monoBehaviour, Action action, float time, 
            float repeatTime)
        {
            return monoBehaviour.StartCoroutine(Routine());

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
    }
}
