using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
#endif

namespace TWizard.Core
{
    /// <summary>
    /// Allows to show a string as a <see cref="SceneAsset"/> object.
    /// </summary>
    public class SceneAttribute : PropertyAttribute
    {
        // public bool OnlyUseBuildScenes { get; set; } = true;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneDrawer : PropertyDrawer
    {
        protected SceneAttribute Attribute => (SceneAttribute)attribute;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            Debug.Log("USING UIELEMENTS");
            if (property.propertyType != SerializedPropertyType.String)
                return new Label("Use [Scene] with strings.");

            var asset = GetSceneObject(property.stringValue);
            var field = new ObjectField(property.displayName)
            {
                allowSceneObjects = false,
                objectType = typeof(SceneAsset),
                value = asset,
                binding = null,
            };
            field.RegisterValueChangedCallback((e) =>
            {
                var sceneName = !!e.newValue ? GetSceneObject(e.newValue.name) : null;
                property.stringValue = !!sceneName ? sceneName.name : "";
            });
            return field;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                SceneAsset sceneObject = GetSceneObject(property.stringValue);
                var scene = EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);
                if (scene == null)
                {
                    property.stringValue = "";
                }
                else if (scene.name != property.stringValue)
                {
                    SceneAsset sceneObj = GetSceneObject(scene.name);
                    if (sceneObj == null)
                    {
                        Debug.LogWarning($"The scene {scene.name} cannot be used. " +
                            "To use this scene add it to the build settings for the project");
                    }
                    else
                    {
                        property.stringValue = scene.name;
                    }
                }
            }
            else EditorGUI.LabelField(position, label.text, "Use [Scene] with strings.");
        }

        protected SceneAsset GetSceneObject(string sceneObjectName)
        {
            if (string.IsNullOrEmpty(sceneObjectName))
                return null;

            foreach (EditorBuildSettingsScene editorScene in EditorBuildSettings.scenes)
            {
                if (editorScene.path.Contains(sceneObjectName))
                    return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
            }
            Debug.LogWarning($"Scene [{sceneObjectName}] cannot be used. Add this scene to the 'Scenes in the Build' in build settings.");
            return null;
        }
    }
#endif
}