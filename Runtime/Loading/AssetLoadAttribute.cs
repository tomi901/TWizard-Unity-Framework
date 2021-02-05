using System;
using System.Reflection;
using UnityEngine;
using System.Threading.Tasks;
#if UNITASK
using Cysharp.Threading.Tasks;
#endif


namespace TWizard.Core
{
    public static class AssetLoadExtensions
    {
        public static AssetLoadAttribute GetLoader(this Type type) => type.GetCustomAttribute<AssetLoadAttribute>();
    }

    /// <summary>
    /// An attribute to tell how this asset class should be loaded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public abstract class AssetLoadAttribute : Attribute
    {
        public bool IgnoredByChecker { get; set; }

        public AssetLoadAttribute()
        {
        }

        public AssetLoadAttribute(bool ignoredByChecker)
        {
            IgnoredByChecker = ignoredByChecker;
        }


        /// <summary>
        /// Load synchronously the asset.
        /// </summary>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>The loaded asset.</returns>
        public virtual T Load<T>() where T : UnityEngine.Object
        {
            throw new NotImplementedException("Synchronous Load not implemented, use LoadAsync instead!");
        }

        /// <summary>
        /// Loads asynchronously the asset in a generic way to work with loaders like <see cref="Resources.LoadAsync(string)"/>
        /// or <see cref="AssetBundle.LoadAssetAsync(string)"/>.
        /// If not overriden, it will use <see cref="Load{T}"/>
        /// </summary>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <param name="onLoaded">Callback when the resource is loaded.</param>
        /// <param name="onError">Callback when the loading gave an error.</param>
        public abstract void LoadAsync<T>(ResultCallback<T> callback, IProgress<Func<float>> progress = null) where T : UnityEngine.Object;

#if UNITASK
        public UniTask<T> LoadAsync<T>(IProgress<Func<float>> progress = null) where T : UnityEngine.Object
        {
            var tcs = new UniTaskCompletionSource<T>();
            LoadAsync<T>((result) =>
            {
                if (result.IsSuccesful)
                    tcs.TrySetResult(result);
                else
                    tcs.TrySetException(result.Exception);
            }, progress);
            return tcs.Task;
        }
#endif
    }
}