using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


namespace TWizard.Core.Async
{
    [HideInInspector]
    public sealed class UnityEvents : MonoBehaviour
    {
        private static UnityEvents instance;
        internal static UnityEvents Instance
        {
            get
            {
                if (instance)
                {
                    var go = new GameObject("Unity Events")
                    {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                    instance = go.AddComponent<UnityEvents>();
                }
                return instance;
            }
        }

        public enum Type
        {
            Update,
            FixedUpdate,
        }


        public static event UnityAction OnUpdate { add => Instance.onUpdate += value; remove => Instance.onUpdate -= value; }
        public static event UnityAction OnFixedUpdate { add => Instance.onFixedUpdate += value; remove => Instance.onFixedUpdate -= value; }


        private UnityAction onUpdate = delegate { },
            onFixedUpdate = delegate { };


        private void Update()
        {
            onUpdate();
        }

        private void FixedUpdate()
        {
            onFixedUpdate();
        }



        public Task AwaitAsCoroutine(System.Collections.IEnumerator enumerator) => AwaitAsCoroutine(enumerator);

        public Task AwaitAsCoroutine(YieldInstruction instruction) => AwaitAsCoroutine(instruction);

        public Task AwaitAsCoroutine(object obj)
        {
            var task = new TaskCompletionSource<bool>();

            StartCoroutine(Routine());
            System.Collections.IEnumerator Routine()
            {
                yield return obj;
                task.SetResult(true);
            }
            return task.Task;
        }


        public void WaitAsCoroutine(object obj, System.Action onFinish)
        {
            if (onFinish == null)
                throw new System.ArgumentNullException(nameof(onFinish));

            StartCoroutine(Routine());
            System.Collections.IEnumerator Routine()
            {
                yield return obj;
                onFinish();
            }
        }
    }
}