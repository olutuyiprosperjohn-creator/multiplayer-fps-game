# API Reference

## REST API

### Authentication

#### Register
```
POST /api/auth/register

Body:
{
  "username": "player1",
  "email": "player@example.com",
  "password": "secure_password"
}

Response: 201 Created
{
  "message": "User registered successfully",
  "user": {
    "id": 1,
    "username": "player1",
    "email": "player@example.com"
  },
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

#### Login
```
POST /api/auth/login

Body:
{
  "email": "player@example.com",
  "password": "secure_password"
}

Response: 200 OK
{
  "message": "Login successful",
  "user": {
    "id": 1,
    "username": "player1",
    "email": "player@example.com"
  },
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

### Players

#### Get Profile
```
GET /api/players/profile
Headers:
  Authorization: Bearer {token}

Response: 200 OK
{
  "id": 1,
  "username": "player1",
  "email": "player@example.com",
  "kills": 145,
  "deaths": 89,
  "wins": 34,
  "losses": 16,
  "rank": 2500
}
```

#### Get Leaderboard
```
GET /api/players/leaderboard

Response: 200 OK
[
  {
    "username": "ProPlayer",
    "kills": 5234,
    "deaths": 1892,
    "wins": 456,
    "losses": 123,
    "rank": 5000
  },
  {
    "username": "player1",
    "kills": 145,
    "deaths": 89,
    "wins": 34,
    "losses": 16,
    "rank": 2500
  },
  ...
]
```

## WebSocket Events

### Client → Server

#### join_queue
```javascript
socket.emit('join_queue', {
  username: 'player1',
  skill: 1500  // skill rating
});
```

#### player_input
```javascript
socket.emit('player_input', {
  position: { x: 10, y: 1, z: 5 },
  rotation: { x: 0, y: 45, z: 0 },
  velocity: { x: 3, y: 0, z: 0 }
});
```

#### fire_weapon
```javascript
socket.emit('fire_weapon', {
  targetSocketId: 'target_player_id',
  weapon: 'rifle',
  distance: 25.5,
  hitLocation: 'head'
});
```

### Server → Client

#### match_found
```javascript
socket.on('match_found', (data) => {
  console.log(data.matchId);  // "match_1234567890"
  console.log(data.match);     // Game state object
});
```

#### player_update
```javascript
socket.on('player_update', (data) => {
  console.log(data.socketId);   // Player's socket ID
  console.log(data.position);   // {x, y, z}
  console.log(data.rotation);   // {x, y, z}
});
```

#### player_hit
```javascript
socket.on('player_hit', (data) => {
  console.log(data.targetId);   // Hit player's ID
  console.log(data.damage);     // Damage dealt
  console.log(data.health);     // Remaining health
});
```

#### player_died
```javascript
socket.on('player_died', (data) => {
  console.log(data.playerId);   // Killed player's ID
  console.log(data.killer);     // Killer's socket ID
});
```
