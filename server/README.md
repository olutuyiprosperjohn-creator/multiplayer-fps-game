# Multiplayer FPS Server

Node.js backend server for the multiplayer FPS game.

## Setup

1. Install dependencies:
```bash
npm install
```

2. Configure environment variables:
```bash
cp .env.example .env
# Edit .env with your configuration
```

3. Setup database:
```bash
npm run migrate
```

4. Start the server:
```bash
npm run dev
```

Server will run on `http://localhost:3000`

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user

### Players
- `GET /api/players/profile` - Get player profile
- `GET /api/players/leaderboard` - Get global leaderboard

## WebSocket Events

### Client -> Server
- `join_queue` - Join matchmaking queue
- `leave_queue` - Leave matchmaking queue
- `player_input` - Send player movement/rotation
- `fire_weapon` - Send weapon fire event

### Server -> Client
- `match_found` - Match has been found
- `player_update` - Another player moved
- `player_hit` - Player was hit by weapon
- `player_died` - Player died
