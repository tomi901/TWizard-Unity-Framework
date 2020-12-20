using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

using TWizard.Core.Async;

namespace TWizard.Core.Loading
{
    /// <summary>
    /// Util static class to allow the await of unity async operations.
    /// </summary>
    public static partial class Load
    {
        public delegate Task TaskOperation();


        private static Scene lastLoadedScene;

        static Load()
        {
            SceneManager.sceneLoaded += (scene, _) => lastLoadedScene = scene;
        }


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
