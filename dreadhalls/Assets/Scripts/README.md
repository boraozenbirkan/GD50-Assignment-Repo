Changes:
- Changed WhisperSound object (DontDestroy) to a Game Manager which serves as a AudioManager and as a data container.
- Turned the Game Manager (DontDestroy) object to a prefab.
- Created Game Over Scene
- Created whole new script called LevelText to display maze number as level number on the screen in Play and Game Over Scene.
- GrabPickups script increases number of maze variable.
- Updated LoadSceneOnInput scprit to use it in Game Over scene as well.
- Updated LevelGenerator script to create holes
- Updated FirstPersonController script to create transition to Game Over scene
