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
            SingletonTest.Load();
            SingletonTest loaded = SingletonTest.Instance;
            Assert.That(loaded, Is.Not.Null);
            Assert.That(SingletonTest.IsLoaded);
        }

        [UnityTest]
        public IEnumerator LoadAsynchronously()
        {
            var task = SingletonTest.LoadAsync();
            yield return task.AsYieldable();

            SingletonTest loaded = SingletonTest.Instance;
            Assert.That(loaded, Is.Not.Null);
            Assert.That(SingletonTest.IsLoaded);
        }
    }


    public class TestLoadAttribute : AssetLoadAttribute
    {
        public int AsyncDelayMiliseconds { get; set; } = 100;

        public override T Load<T>()
        {
            if (typeof(T).IsSubclassOf(typeof(ScriptableObject)))
                return ScriptableObject.CreateInstance(typeof(T)) as T;
            else
                return Activator.CreateInstance<T>();
        }

        public override async Task<T> LoadAsync<T>(IProgress<Func<float>> progress = null)
        {
            await Task.Delay(AsyncDelayMiliseconds);
            return Load<T>();
        }
    }

    [TestLoad]
    public class SingletonTest : ScriptableObjectSingleton<SingletonTest>
    {
    }
}
