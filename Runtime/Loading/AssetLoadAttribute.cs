using System;


namespace TWizard.Framework
{
    /// <summary>
    /// An attribute to tell how this asset class should be loaded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public abstract class AssetLoadAttribute : Attribute
    {
        public AssetLoadAttribute()
        {
        }

        /// <summary>
        /// Load synchronously the asset.
        /// </summary>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>The loaded asset.</returns>
        public abstract T Load<T>() where T : UnityEngine.Object;

        /// <summary>
        /// Loads asynchronously the asset in a generic way to work with loaders like <see cref="UnityEngine.Resources.LoadAsync(string)"/>
        /// or <see cref="UnityEngine.AssetBundle.LoadAssetAsync(string)"/>.
        /// If not overriden, it will use <see cref="Load{T}"/>
        /// </summary>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <param name="onComplete">Callback when the resource is loaded.</param>
        /// <param name="onError">Callback when the loading gave an error.</param>
        public virtual void LoadAsync<T>(Action<T> onComplete, Action<Exception> onError) where T : UnityEngine.Object
        {
            try
            {
                onComplete?.Invoke(Load<T>());
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
            }
        }
    }
}