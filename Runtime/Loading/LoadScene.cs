using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace TWizard.Core
{
    /// <summary>
    /// Util static class to allow the await of unity async operations.
    /// </summary>
    public static partial class Load
    {
        #region AsyncOperation await
        public struct AsyncOperationAwaiter : INotifyCompletion
        {
            public AsyncOperation operation;

            public bool IsCompleted => operation.isDone;

            public AsyncOperationAwaiter(AsyncOperation operation)
            {
                this.operation = operation;
            }

            public void OnCompleted(Action continuation)
            {
                operation.completed += (op) => continuation();
            }

            public AsyncOperation GetResult() => operation;
        }

        public static AsyncOperationAwaiter GetAwaiter(this AsyncOperation operation) => new AsyncOperationAwaiter(operation);
        #endregion


        private static Scene lastLoadedScene;

        static Load()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            lastLoadedScene = scene;
        }


        public delegate Task TaskOperation();

        public static async Task<Scene> Scene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single,
            TaskOperation postLoad = null)
        {
            await SceneManager.LoadSceneAsync(sceneName, mode);
            Scene loadedScene = lastLoadedScene;

            if (postLoad != null)
            {
                // Deactivate the objects to allow post load
                var sceneObjects = new List<GameObject>();
                loadedScene.GetRootGameObjects(sceneObjects);
                sceneObjects.RemoveAll(go => !go.activeSelf);

                foreach (GameObject obj in sceneObjects)
                    obj.SetActive(false);

                // Do all the post-load operations while awaiting them
                foreach (TaskOperation operation in postLoad.GetInvocationList())
                    await operation();

                // Activate the root objects that were deactivated
                foreach (GameObject obj in sceneObjects)
                    obj.SetActive(true);
            }
            return loadedScene;
        }
    }
}
