# TWizard's Unity Framework

General framework I use for my Unity projects.

## Features

### ScriptableObjectSingleton

A singleton for *ScriptableObjects*, useful for custom project settings editable in the inspector.
Must be used with a *AssetLoadAttribute* to handle the loading. See "AssetLoadAttribute" section for more details.

```C#
// Loads this singleton from a resource located in Resources/Game Configuration
[CreateAssetMenu(FileName = "Game Configuration")]
[ResourceLoadAttribute("Game Configuration")]
public class GameConfiguration : ScriptableObjectSingleton<GameConfiguration>
{
...
```

### AssetLoadAttribute

An attribute to place to a certain class and define how we should load an Asset of that class, its abstract so you should use the inheriting classes:

* `[ResourceLoad(string path)]`: Loads the asset at that resources path. Must be place inside a **Resources** folder.
* `[AddressableLoad(string key)]`: When including the Unity Addressables package, must set an asset with the addresable key.

### SceneAttribute

A fancy way to show a string as a *SceneAsset* and avoid hardcoded strings. Useful for storing the scenes in
*ScriptableObjectSingleton*

```C#
...
// Will be show on the inspector as a SceneAsset
[SerializeField, Scene]
private string sceneName;
...
```

### Load.Scene

Loads a scene asynchronously with a *ResultCallback<Scene>* or a *UniTask<Scene>* returning the loaded *Scene*.

## Installing

In your Unity project inside the *Packages* folder modify your manifest.json and a line in the *dependencies* field:

```json
"com.twizard.core": "https://github.com/tomi901/TWizard-Unity-Framework.git"
```

(TODO: Use a scopedRegistry)
