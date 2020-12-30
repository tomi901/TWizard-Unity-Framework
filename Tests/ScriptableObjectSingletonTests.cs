using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using TWizard.Core.Loading;

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
            SingletonTest loaded = null;
            SingletonTest.LoadAsync((result) => {
                if (result.IsSuccesful)
                    loaded = result;
                else
                    Debug.LogException(result.Exception);
            });
            yield return new WaitUntil(() => !!loaded);

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
    }

    [TestLoad]
    public class SingletonTest : ScriptableObjectSingleton<SingletonTest>
    {
    }
}
