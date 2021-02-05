using System;
using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using TWizard.Core.Async;
using UnityEngine;
using UnityEngine.TestTools;

namespace TWizard.Core.Tests
{
    public class ScriptableObjectSingletonTests
    {
        [Test]
        public void LoadSynchronously()
        {
            var loaded = SingletonTest.Load();
            Assert.That(loaded, Is.Not.Null);
            Assert.That(loaded, Is.EqualTo(SingletonTest.Instance));
            Assert.That(SingletonTest.IsLoaded);
        }

        [UnityTest]
        public IEnumerator LoadAsynchronously()
        {
            var listener = new ResultListener<SingletonTest>();
            SingletonTest.LoadAsync(listener.Callback);
            yield return listener;

            SingletonTest loaded = listener.Value;
            Assert.That(loaded, Is.Not.Null);
            Assert.That(loaded, Is.EqualTo(SingletonTest.Instance));
            Assert.That(SingletonTest.IsLoaded);
        }
    }


    public class TestLoadAttribute : AssetLoadAttribute
    {
        public override T Load<T>()
        {
            if (typeof(T).IsSubclassOf(typeof(ScriptableObject)))
                return ScriptableObject.CreateInstance(typeof(T)) as T;
            else
                return Activator.CreateInstance<T>();
        }

        public override async void LoadAsync<T>(ResultCallback<T> callback, IProgress<Func<float>> progress = null)
        {
            await Task.Delay(500);
            callback.SetResult(Load<T>());
        }
    }

    [TestLoad]
    public class SingletonTest : ScriptableObjectSingleton<SingletonTest>
    {
    }
}
