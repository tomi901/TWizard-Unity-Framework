#if UNITASK
using UnityEngine;
using Cysharp.Threading.Tasks;
#endif


namespace TWizard.Core
{
    public static partial class Load
    {
#if UNITASK
        public static async UniTask<T> Resource<T>(string path, System.IProgress<float> progress = null, PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update,
            System.Threading.CancellationToken cancellationToken = default)
            where T : Object
        {
            var request = Resources.LoadAsync<T>(path);
            await request.ToUniTask(progress, playerLoopTiming, cancellationToken);
            return (T)request.asset;
        }
#endif
    }
}
