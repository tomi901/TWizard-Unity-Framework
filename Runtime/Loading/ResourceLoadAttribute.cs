using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace TWizard.Core
{
    /// <summary>
    /// An attribute to tell to load an instance as a resource into a specified resource path defined by <see cref="Path"/>
    /// </summary>
    public class ResourceLoadAttribute : AssetLoadAttribute
    {
        public string Path { get; }


        public ResourceLoadAttribute(string path)
        {
            Path = path;
        }

        public override T Load<T>() => Resources.Load<T>(Path);

        public override async Task<T> LoadAsync<T>(IProgress<Func<float>> progress = null)
        {
            var request = Resources.LoadAsync<T>(Path);
            progress?.Report(() => request.progress);
            var asset = await request;
            if (!asset)
                throw new Exception($"Resource on path \"{Path}\" not found.");

            return (T)asset;
        }
    }

    internal static class ResourceAwaiter
    {
        public struct Awaiter : INotifyCompletion
        {
            private readonly ResourceRequest request;
            public bool IsCompleted => request.isDone;

            public Awaiter(ResourceRequest request) => this.request = request;

            public void OnCompleted(Action continuation)
            {
                request.completed += (_) => continuation();
            }

            public UnityEngine.Object GetResult() => request.asset;
        }

        public static Awaiter GetAwaiter(this ResourceRequest request) => new Awaiter(request);
    }
}