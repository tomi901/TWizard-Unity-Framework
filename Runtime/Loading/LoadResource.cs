using UnityEngine;
#if UNITASK
using Cysharp.Threading.Tasks;
#endif


namespace TWizard.Core
{
    public static partial class Load
    {
#if UNITASK
        public static async UniTask<T> Resource<T>(string path, System.IProgress<float> progress = null, PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update)
            where T : Object
        {
            var request = Resources.LoadAsync<T>(path);
            await request.ToUniTask(progress, playerLoopTiming);
            return (T)request.asset;
        }
#endif
    }
}
