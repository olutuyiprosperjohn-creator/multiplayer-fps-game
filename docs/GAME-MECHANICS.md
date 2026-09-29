# Game Mechanics

## Weapons System

### Weapon Stats

| Weapon  | Damage | Fire Rate | Magazine | Range | Accuracy |
|---------|--------|-----------|----------|-------|----------|
| Rifle   | 30     | 10 RPM    | 30       | 100m  | 0.85     |
| Pistol  | 15     | 12 RPM    | 15       | 50m   | 0.70     |
| Shotgun | 50     | 2 RPM     | 8        | 20m   | 0.60     |
| Sniper  | 80     | 1 RPM     | 10       | 200m  | 0.95     |

### Hit Locations & Multipliers

- **Head**: 2.0x damage (critical)
- **Torso**: 1.0x damage (normal)
- **Arms**: 0.6x damage (reduced)
- **Legs**: 0.75x damage (reduced)

### Damage Formula

```
Final Damage = Base Damage × Distance Falloff × Hit Multiplier
Distance Falloff = max(0.3, 1 - (distance / max_range))
```

## Movement

- **Walk Speed**: 5 m/s
- **Sprint Speed**: 8 m/s
- **Jump Height**: 2 meters
- **Air Control**: Limited during jump

## Health & Survival

- **Starting Health**: 100 HP
- **Max Health**: 100 HP
- **Health Packs**: Restore 50 HP
- **Respawn Time**: 5 seconds

## Scoring System

- **Kill**: +100 points
- **Assist**: +25 points
- **Headshot Kill**: +150 points
- **Double Kill**: +200 points (2 kills within 3 seconds)
- **Win Round**: +500 points

## Team Gameplay

- **Team Size**: 2-8 players per team
- **Teams**: Blue vs Red
- **Objective**: Eliminate enemy team or reach score limit
- **Match Duration**: 10 minutes default

## Progression

- **Levels**: 1-100
- **Experience**: Earned from matches
- **Ranking**: Based on win rate and K/D ratio
- **Unlocks**: Weapons, skins, emotes at certain levels
