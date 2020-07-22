using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace TWizard.Framework
{
    [HideInInspector]
    public class AwaitCoroutine : MonoBehaviour
    {
        private static AwaitCoroutine main;
        public static AwaitCoroutine Main
        {
            get
            {
                if (!main)
                {
                    main = CreateAwaiter("Awaitable Coroutines");
                    main.gameObject.hideFlags = HideFlags.HideAndDontSave;
                    DontDestroyOnLoad(main);
                }
                return main;
            }
        }

        private AwaitCoroutine() { }

        public static AwaitCoroutine CreateAwaiter(string name) => new GameObject(name).AddComponent<AwaitCoroutine>();


        public static Task MainExecute(IEnumerator routine, CancellationToken cancellationToken = default)
        {
            return Main.Execute(routine, cancellationToken);
        }

        public async Task Execute(IEnumerator routine, CancellationToken cancellationToken = default)
        {
            using (var signal = new SemaphoreSlim(0, 1))
            {
                Coroutine coroutine = StartCoroutine(NotifiedRoutine());
                cancellationToken.Register(() =>
                {
                    // Debug.Log("Cancel", this);
                    StopCoroutine(coroutine);
                });
                await signal.WaitAsync(cancellationToken);

                IEnumerator NotifiedRoutine()
                {
                    yield return routine;
                    signal.Release();
                }
            }
        }
    }
}
