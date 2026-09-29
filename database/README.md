# Database Schema

PostgreSQL database schema for the multiplayer FPS game.

## Tables

- **users** - Player account information
- **player_stats** - Game statistics and ranking
- **match_history** - Historical match data
- **weapons** - Weapon definitions
- **player_weapons** - Player weapon inventory

## Setup

1. Create database:
```bash
creatdb multiplayer_fps
```

2. Load schema:
```bash
psql multiplayer_fps < schema.sql
```

3. (Optional) Seed test data:
```bash
psql multiplayer_fps < seeds.sql
```
