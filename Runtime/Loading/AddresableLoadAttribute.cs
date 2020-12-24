#if USING_ADDRESABLES
using System;
using UnityEngine.AddressableAssets;


namespace TWizard.Core.Loading
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

        public override void LoadAsync<T>(Action<T> onLoaded, Action<Exception> onError)
        {
            Addressables.LoadAssetAsync<T>(Key).Completed += (op) =>
            {
                if (op.OperationException != null)
                    onError(op.OperationException);
                else
                    onLoaded(op.Result);
            };
        }
    }
}
#endif