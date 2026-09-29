# Multiplayer FPS Client

Unity game client for the multiplayer FPS game.

## Setup

1. Open Unity Hub
2. Create new project (3D, URP recommended)
3. Import the project files
4. Install Socket.IO client package:
   - Window > TextMesh Pro > Import TMP Essential Resources
   - Add Socket.IO package via Package Manager

## Project Structure

```
Assets/
├── Scripts/
│   ├── Networking/
│   ├── Player/
│   ├── Weapons/
│   ├── UI/
│   └── Game/
├── Scenes/
│   ├── MainMenu.unity
│   ├── Lobby.unity
│   └── GameScene.unity
├── Prefabs/
│   ├── Player.prefab
│   ├── Weapon.prefab
│   └── UI/
└── Resources/
    └── Weapons/
```

## Key Scripts

- `NetworkManager.cs` - Socket.IO connection
- `PlayerController.cs` - Player movement and input
- `WeaponSystem.cs` - Weapon mechanics
- `GameManager.cs` - Match state management
- `UIManager.cs` - UI handling

## Development

Play in editor with F5 or Build > Build And Run
