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

        public override void LoadAsync<T>(Action<T> onLoaded, Action<Exception> onError)
        {
            var request = Resources.LoadAsync<T>(Path);
            request.completed += (_) =>
            {
                T asset = (T)request.asset;
                if (!!asset)
                    onLoaded?.Invoke((T)request.asset);
                else
                    onError?.Invoke(new Exception($"Couldn't load asset in path: {Path}"));
            };
        }
    }
}