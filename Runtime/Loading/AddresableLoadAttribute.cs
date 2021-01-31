#if USING_ADDRESABLES
using System;
using UnityEngine.AddressableAssets;


namespace TWizard.Core
{
    public class AddresableLoadAttribute : AssetLoadAttribute
    {
        public string Key { get; }

        public AddresableLoadAttribute(string key) => Key = key;


        public override T Load<T>()
        {
            var operation = Addressables.LoadAssetAsync<T>(Key);
            operation.Task.Wait();
            return operation.Result;
        }

        public override void LoadAsync<T>(ResultCallback<T> callback, IProgress<Func<float>> progress = null)
        {
            var operation = Addressables.LoadAssetAsync<T>(Key);
            progress?.Report(() => operation.PercentComplete);
            operation.Completed += (op) =>
            {
                if (op.OperationException != null)
                    callback.SetException(op.OperationException);
                else
                    callback.SetResult(op.Result);
            };
        }
    }
}
#endif