using System;
using UnityEngine;

namespace TWizard.Core
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

        public override void LoadAsync<T>(ResultCallback<T> callback, IProgress<Func<float>> progress = null)
        {
            var request = Resources.LoadAsync<T>(Path);
            progress?.Report(() => request.progress);
            request.completed += (_) =>
            {
                T asset = (T)request.asset;
                if (!!asset)
                    callback.SetResult(asset);
                else
                    callback.SetException(new Exception($"Couldn't load asset in path: {Path}"));
            };
        }
    }
}