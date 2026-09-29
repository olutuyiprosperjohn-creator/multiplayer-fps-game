# System Architecture

## Overview

```
┌─────────────────┐          ┌──────────────────────┐
│  Game Client    │          │  Web Server          │
│  (Unity)        │◄────────►│  (Node.js/Express)   │
│                 │          │  - Authentication    │
│ - FPS Gameplay  │          │  - REST APIs         │
│ - Input Handling│          │  - WebSocket Server  │
│ - Rendering     │          └──────────────────────┘
└─────────────────┘                    │
                                       │
                        ┌──────────────▼──────────────┐
                        │    PostgreSQL Database      │
                        │  - Users                    │
                        │  - Stats                    │
                        │  - Match History            │
                        └─────────────────────────────┘
```

## Communication Flow

### 1. Authentication
1. Client sends login/register request to HTTP API
2. Server validates credentials and returns JWT token
3. Client stores token and connects to WebSocket

### 2. Matchmaking
1. Client emits `join_queue` event via WebSocket
2. Server adds player to matchmaking queue
3. When enough players are ready, server creates match
4. Server emits `match_found` to all players in match

### 3. Gameplay
1. Player sends `player_input` events (movement, rotation)
2. Server broadcasts to other players in match
3. Player fires weapon with `fire_weapon` event
4. Server performs hit detection and damage calculation
5. Server broadcasts `player_hit` or `player_died` events

## Server Architecture

### Components

**NetworkManager**
- Socket.IO connection handling
- Event routing
- Player state synchronization

**MatchmakingService**
- Player queue management
- Match creation
- Team balancing

**GameService**
- Player input handling
- Weapon fire processing
- Damage calculation
- Game state management

**Database Layer**
- User authentication
- Player statistics tracking
- Match history storage

## Data Models

### Match State
```javascript
{
  id: "match_1234",
  startTime: 1234567890,
  teams: {
    team1: [
      { socketId: "abc123", username: "player1", health: 100, kills: 2 },
      { socketId: "def456", username: "player2", health: 75, kills: 1 }
    ],
    team2: [...]
  },
  score: { team1: 15, team2: 12 }
}
```

### Player Input
```javascript
{
  position: { x: 10, y: 1, z: 5 },
  rotation: { x: 0, y: 45, z: 0 },
  velocity: { x: 3, y: 0, z: 0 }
}
```

## Networking Details

### WebSocket Events

**Client → Server**
- `join_queue` - Enter matchmaking
- `player_input` - Movement updates (~60 fps)
- `fire_weapon` - Weapon fire request
- `player_died` - Respawn request

**Server → Client**
- `match_found` - Match started
- `player_update` - Other player moved
- `player_hit` - Player took damage
- `player_died` - Player eliminated
- `match_end` - Match finished

### Update Rate
- Client sends input: 60 times per second
- Server broadcasts updates: 60 times per second
- Tick rate can be configured via `TICK_RATE` env var

## Security Considerations

1. **Authentication**: JWT tokens with expiration
2. **Validation**: Server validates all player input
3. **Cheat Prevention**: Server performs hit detection (not client)
4. **Rate Limiting**: Implement rate limiting on API endpoints
5. **Encryption**: Use HTTPS/WSS in production
