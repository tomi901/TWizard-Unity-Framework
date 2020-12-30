using System;
using UnityEngine;

using TWizard.Core.Loading;


namespace TWizard.Core
{
    /// <summary>
    /// To make an object of type <see cref="ScriptableObject"/> to be able to be loaded
    /// synchronously or asynchronously and be stored on the property <see cref="Instance"/>.
    /// Useful for creating a config asset.
    /// <para>IMPORTANT: Add an <see cref="AssetLoadAttribute"/> subclass attribute on the inheriting class to handle the loading.</para>
    /// For example you could use an <see cref="ResourceLoadAttribute"/> and set the path where it should be loaded.
    /// </summary>
    /// <typeparam name="T">The type of the Singleton object, should match the inheriting class.</typeparam>
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T>
    {
        private static T instance;
        /// <summary>
        /// Gets the instance, if not loaded it will call the <see cref="Load"/>
        /// </summary>
        public static T Instance
        {
            get => Load();
            protected set => instance = value;
        }
        /// <summary>
        /// If the <see cref="Instance"/> is already loaded.
        /// </summary>
        public static bool IsLoaded => instance != null;


        private static AssetLoadAttribute GetLoader()
        {
            var loader = typeof(T).GetLoader();
            if (loader == null)
                throw new Exception($"No {nameof(AssetLoadAttribute)} assigned, add a subclass attribute on the class.");

            return loader;
        }

        public static T Load()
        {
            if (!IsLoaded)
            {
                var loader = GetLoader();
                Debug.Log($"Loading synchronously singleton of type '{nameof(T)}'...");
                instance = loader.Load<T>();
            }
            return instance;
        }

        public static void LoadAsync(Action<Result<T>> callback = null)
        {
            // Already has an instance, return and call onLoaded
            if (IsLoaded)
            {
                callback?.SetResult(instance);
                return;
            }

            var loader = GetLoader();
            Debug.Log($"Loading asynchronously singleton of type '{nameof(T)}'...");
            loader.LoadAsync<T>((result) =>
            {
                if (result.IsSuccesful)
                {
                    instance = result.Value;
                    callback?.SetResult(instance);
                }
                else
                {
                    callback?.SetException(result.Exception);
                }
            });
        }
    }
}
