using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TWizard.Core.Tests
{
    public class AwaitCoroutineTests
    {
        [UnityTest]
        public IEnumerator AwaitBasicCoroutine()
        {
            bool executed = false;
            IEnumerator Routine()
            {
                yield return null;
                executed = true;
            }

            var task = AwaitCoroutine.MainExecute(Routine());
            yield return new WaitUntil(() => task.IsCompleted || task.IsFaulted || task.IsCanceled);

            // Assert
            Assert.That(executed, Is.True);
        }

        [UnityTest]
        public IEnumerator CanCancel()
        {
            IEnumerator Routine()
            {
                yield return new WaitWhile(() => true);
            }

            System.Threading.Tasks.Task task;
            using (var cancelation = new System.Threading.CancellationTokenSource())
            {
                task = AwaitCoroutine.MainExecute(Routine(), cancelation.Token);
                yield return null;
                cancelation.Cancel();
                yield return new WaitUntil(() => task.IsCompleted || task.IsFaulted || task.IsCanceled);
            }
            Assert.That(task.IsCanceled, Is.True);
        }
    }
}
