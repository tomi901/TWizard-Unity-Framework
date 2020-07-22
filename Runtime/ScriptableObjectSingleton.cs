using System;
using System.Reflection;
using UnityEngine;

namespace TWizard.Framework
{
    /// <summary>
    /// To make an object of type <see cref="ScriptableObject"/> to be able to be loaded as a resource
    /// and act as a resource and stored on the property <see cref="Instance"/>. <para>IMPORTANT:
    /// To define the resource path add "new const string ResourcePath = (Resource Path);".</para>
    /// </summary>
    /// <typeparam name="T">The type of the Singleton object.</typeparam>
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T>
    {
        /// <summary>
        /// The constant 
        /// </summary>
        protected const string ResourcePath = null;

        private static T instance;
        public static T Instance => Load();

        public static bool Loaded => instance != null;


        private static string GetResourceName()
        {
            const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            // Try to check if we overriden the ResourceName const by doing 'new const string ResourceName',
            // if not found, throw an exception.
            return typeof(T).GetField(nameof(ResourcePath), flags)?.GetRawConstantValue() as string ??
                throw new NotImplementedException($"No string constant with name '{nameof(ResourcePath)}' " +
                $"overridden with type '{typeof(T)}'.");
        }

        public static T Load()
        {
            if (!Loaded)
            {
                string name = GetResourceName();
                Debug.Log($"Loading synchronously resource '{name}'...");
                instance = Resources.Load<T>(name);

                if (!Loaded)
                    throw new Exception($"No resource with name '{name}' of type '{typeof(T)}' found.");
            }

            return instance;
        }

        public static ResourceRequest LoadAsync()
        {
            if (Loaded) return null;

            string name = GetResourceName();
            Debug.Log($"Loading asynchronously resource '{name}'...");
            ResourceRequest request = Resources.LoadAsync<T>(name);

            // When the loading is complete, assign the instance.
            request.completed += (op => instance = (T)request.asset);
            return request;
        }
    }
}