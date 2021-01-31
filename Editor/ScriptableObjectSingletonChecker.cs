using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;


namespace TWizard.Core.Editor
{
    public static class ScriptableObjectSingletonChecker
    {
        private const BindingFlags StaticFlags = BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;


        [InitializeOnLoadMethod]
        [MenuItem("Tools/TWizard/Check Scriptable Object Singletons")]
        public static void Check()
        {
            // Debug.Log($"Currently found ScriptableObjectSingletons:\n{string.Join("\n *", GetInheritedTypes())}");
            foreach (Type type in GetInheritedTypes())
            {
                if (!HasAssetLoadAttribute(type))
                    Debug.LogError($"ScriptableObjectSingleton \"{type}\" has no [{nameof(AssetLoadAttribute)}] attribute.");


                // var loadMethod = type.GetMethod("Load", StaticFlags);
                // if (loadMethod.ReturnType != type)
                //     Debug.LogError($"ScriptableObjectSingleton \"{type}\" generic is not the same type.");

                try
                {
                    CanLoad(type);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error loading ScriptableObjectSingleton \"{type}\": {e.Message}.");
                    Debug.LogException(e);
                }
            }
        }


        public static IEnumerable<Type> GetInheritedTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany((a) => a.GetTypes())
                .Where(IsAnInheritingClass);
        }

        public static bool IsAnInheritingClass(Type type)
        {
            return !type.IsAbstract && type.IsSubclassOf(typeof(ScriptableObjectSingleton));
        }

        public static bool HasAssetLoadAttribute(Type type) => type.GetLoader() != null;

        /*
        public static void CreateAsset(Type type)
        {
            var loader = type.GetLoader();
            string path = loader.CreateAssetOnPath;
            if (path.EndsWith("/"))
                path += "UnnamedAsset.asset";
            else if (!path.EndsWith(".asset"))
                path += ".asset";

            var asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
        */

        public static void CanLoad(Type type)
        {
            // Debug.Log($"{type}: {string.Join(", ", type.GetProperties(StaticFlags).Select((m) => m.Name))}");
            var instanceProperty = type.GetProperty("Instance", StaticFlags);
            instanceProperty.SetValue(null, null); // Set it to null to assert we are loading correctly

            object loaded = type.GetMethod("Load", StaticFlags).Invoke(null, new object[0]);
            if (loaded == null)
                throw new Exception("Load returned null.");
        }
    }
}
