using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITASK
using Cysharp.Threading.Tasks;
#endif


namespace TWizard.Core
{
    /// <summary>
    /// Util static class to allow the await of unity async operations.
    /// </summary>
    public static partial class Load
    {
#if UNITASK
        public static async UniTask<Scene> Scene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single,
            IProgress<float> progress = null, PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update)
        {
            Scene loadedScene = default;
            try
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                await SceneManager.LoadSceneAsync(sceneName, mode).ToUniTask(progress, playerLoopTiming);
                return loadedScene;
            }
            catch
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                throw;
            }

            void OnSceneLoaded(Scene s, LoadSceneMode m)
            {
                if (m == mode && s.name == sceneName)
                {
                    loadedScene = s;
                    SceneManager.sceneLoaded -= OnSceneLoaded;
                }
            }
        }
#endif

        public static AsyncOperation Scene(string sceneName, ResultCallback<Scene> onLoad, LoadSceneMode mode = LoadSceneMode.Single)
        {
            Scene loadedScene = default;
            try
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                var operation = SceneManager.LoadSceneAsync(sceneName, mode);
                operation.completed += AfterLoad;
                return operation;
            }
            catch 
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                throw;
            }

            void AfterLoad(AsyncOperation _)
            {
                onLoad.SetResult(loadedScene);
            }

            void OnSceneLoaded(Scene s, LoadSceneMode m)
            {
                if (m == mode && s.name == sceneName)
                {
                    loadedScene = s;
                    SceneManager.sceneLoaded -= OnSceneLoaded;
                }
            }
        }
    }
}
