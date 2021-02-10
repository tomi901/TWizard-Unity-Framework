#if USING_ADDRESABLES
using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;


namespace TWizard.Core
{
    public class AddresableLoadAttribute : AssetLoadAttribute
    {
        public string Key { get; }

        public AddresableLoadAttribute(string key) => Key = key;


        public override Task<T> LoadAsync<T>(IProgress<Func<float>> progress = null)
        {
            var operation = Addressables.LoadAssetAsync<T>(Key);
            progress?.Report(() => operation.PercentComplete);
            return operation.Task;
        }
    }
}
#endif