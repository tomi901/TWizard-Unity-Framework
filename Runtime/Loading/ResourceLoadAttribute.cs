using System;
using UnityEngine;

namespace TWizard.Framework
{
    /// <summary>
    /// An attribute to tell to load an instance as a resource into a specified resource path defined by <see cref="Path"/>
    /// </summary>
    public class ResourceLoadAttribute : AssetLoadAttribute
    {
        public string Path { get; }

        public ResourceLoadAttribute()
        {
        }

        public override T Load<T>() => Resources.Load<T>(Path);

        public override void LoadAsync<T>(Action<T> onComplete, Action<Exception> onError)
        {
            var request = Resources.LoadAsync<T>(Path);
            request.completed += (_) =>
            {
                onComplete?.Invoke((T)request.asset);
            };
        }
    }
}