# Multiplayer FPS Game

A full-stack online multiplayer first-person shooter game with server-based architecture.

## Project Structure

```
multiplayer-fps-game/
├── server/              # Node.js backend server
├── client/              # Unity game client
├── shared/              # Shared types and utilities
├── database/            # Database schemas and migrations
├── deployment/          # Docker and deployment configs
└── docs/                # Project documentation
```

## Tech Stack

- **Client**: Unity (C#)
- **Server**: Node.js (Express, Socket.io)
- **Database**: PostgreSQL
- **Networking**: WebSockets (Socket.io)
- **Deployment**: Docker, Docker Compose

## Features

- User authentication and account management
- Matchmaking and lobby system
- Real-time multiplayer gameplay
- Player movement and weapon systems
- Damage and scoring mechanics
- Team-based gameplay modes
- Leaderboards and player stats

## Getting Started

See individual README files in `server/` and `client/` directories.
