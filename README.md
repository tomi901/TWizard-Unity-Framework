# TWizard's Unity Framework

General framework I use for my Unity projects.

## Features

### ScriptableObjectSingleton

A singleton for *ScriptableObjects*, useful for custom project settings editable in the inspector.
Must be used with a *AssetLoadAttribute* to handle the loading.

```C#
// Loads this singleton from a resource located in Resources/Game Configuration
[CreateAssetMenu(FileName = "Game Configuration")]
[ResourceLoadAttribute("Game Configuration")]
public class GameConfiguration : ScriptableObjectSingleton<GameConfiguration>
{
...
```

### SceneAttribute

A fancy way to show a string as a *SceneAsset* and avoid hardcoded strings. Useful for storing the scenes in
*ScriptableObjectSingleton*


### Load.Scene

Loads a scene asynchronously as a *Task<Scene>* returning the loaded *Scene*, allowing extra post-load tasks.
  
*WIP: Progress reporting*

## Installing

In your Unity project inside the *Packages* folder modify your manifest.json and a line in the *dependencies* field:

```json
"com.twizard.core": "https://github.com/tomi901/TWizard-Unity-Framework.git"
```

(TODO: Use a scopedRegistry)
