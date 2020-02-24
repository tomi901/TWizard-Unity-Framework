using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Interbrain.Utils
{
    /// <summary>
    /// Class to handle async loading with a scene as a loading screen.
    /// </summary>
    public class LoadingScreenManager : MonoBehaviour
    {
        private static LoadingScreenManager instance = null;
        public static bool IsLoading => instance != null;

        private static AsyncOperation operation;
        public static float Progress => operation?.progress ?? 0f;


        private readonly List<GameObject> tempRootObjects = new List<GameObject>();
        private readonly Dictionary<GameObject, bool> objectActivations = new Dictionary<GameObject, bool>();


        public static Coroutine Load(string sceneToLoadName, string loadingScreenSceneName,
            IEnumerator postLoad = null,
            IEnumerator preLoad = null)
        {
            if (IsLoading)
                throw new InvalidOperationException("Already loading a scene!");

            instance = new GameObject("Loading Manager").AddComponent<LoadingScreenManager>();
            DontDestroyOnLoad(instance);

            return instance.StartCoroutine(LoadCoroutine());
            IEnumerator LoadCoroutine()
            {
                // Start by loading the loading screen scene synchronously
                SceneManager.LoadScene(loadingScreenSceneName);
                yield return null;
                Scene loadingScreenScene = SceneManager.GetActiveScene();

                yield return preLoad; // Pre-load routine

                // Load asynchronously the target scene and store the reference
                operation = SceneManager.LoadSceneAsync(sceneToLoadName, LoadSceneMode.Additive);
                yield return operation;
                Scene loadedScene = SceneManager.GetSceneByName(sceneToLoadName);

                // Deactivate all the objects of that scene for the post-load
                instance.tempRootObjects.Clear();
                loadedScene.GetRootGameObjects(instance.tempRootObjects);

                instance.objectActivations.Clear();
                foreach (GameObject obj in instance.tempRootObjects) // Cache activation values
                    instance.objectActivations.Add(obj, obj.activeSelf);

                foreach (GameObject obj in instance.tempRootObjects)
                    obj.SetActive(false);


                yield return postLoad; // Post-load routine

                // After the post-load, set the target scene as active, and unload the loading screen scene
                SceneManager.SetActiveScene(loadedScene);
                yield return SceneManager.UnloadSceneAsync(loadingScreenScene);
                foreach (var activation in instance.objectActivations) // Re-active objects with their original value
                    activation.Key.SetActive(activation.Value);

                // Clean the objects and references
                Destroy(instance.gameObject);
                instance = null;
                operation = null;
            }
        }

    }
}
