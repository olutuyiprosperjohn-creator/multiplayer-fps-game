const matches = new Map();

class GameService {
  handlePlayerInput(socketId, inputData) {
    // Find match containing this player
    for (let [matchId, match] of matches) {
      const player = match.gameState.teams.team1.find(p => p.socketId === socketId) ||
                     match.gameState.teams.team2.find(p => p.socketId === socketId);
      
      if (player) {
        // Update player position and rotation
        Object.assign(player, {
          position: inputData.position,
          rotation: inputData.rotation,
          velocity: inputData.velocity
        });

        // Broadcast to other players in match
        this.broadcastToMatch(matchId, 'player_update', {
          socketId: socketId,
          position: inputData.position,
          rotation: inputData.rotation
        });
      }
    }
  }

  handleWeaponFire(socketId, fireData) {
    // Find match containing this player
    for (let [matchId, match] of matches) {
      const playerTeam = match.gameState.teams.team1.find(p => p.socketId === socketId) ? 'team1' : 'team2';
      const targetTeam = playerTeam === 'team1' ? 'team2' : 'team1';
      const targetPlayer = match.gameState.teams[targetTeam].find(p => p.socketId === fireData.targetSocketId);

      if (targetPlayer) {
        // Calculate damage
        const damage = this.calculateDamage(fireData.weapon, fireData.distance, fireData.hitLocation);
        targetPlayer.health -= damage;

        // Find attacker
        const attacker = match.gameState.teams[playerTeam].find(p => p.socketId === socketId);
        attacker.kills += 1;

        if (targetPlayer.health <= 0) {
          targetPlayer.deaths += 1;
          this.broadcastToMatch(matchId, 'player_died', {
            playerId: fireData.targetSocketId,
            killer: socketId
          });
        } else {
          this.broadcastToMatch(matchId, 'player_hit', {
            targetId: fireData.targetSocketId,
            damage: damage,
            health: targetPlayer.health
          });
        }
      }
    }
  }

  calculateDamage(weapon, distance, hitLocation) {
    const weaponDamage = {
      'rifle': 30,
      'pistol': 15,
      'shotgun': 50,
      'sniper': 80
    };

    const hitMultiplier = {
      'head': 2.0,
      'torso': 1.0,
      'legs': 0.75,
      'arms': 0.6
    };

    const baseDamage = weaponDamage[weapon] || 20;
    const distanceFalloff = Math.max(0.3, 1 - (distance / 100));
    const locationMultiplier = hitMultiplier[hitLocation] || 1.0;

    return Math.round(baseDamage * distanceFalloff * locationMultiplier);
  }

  removePlayer(socketId) {
    for (let [matchId, match] of matches) {
      const teamIndex = match.gameState.teams.team1.findIndex(p => p.socketId === socketId);
      if (teamIndex !== -1) {
        match.gameState.teams.team1.splice(teamIndex, 1);
        return;
      }
      const team2Index = match.gameState.teams.team2.findIndex(p => p.socketId === socketId);
      if (team2Index !== -1) {
        match.gameState.teams.team2.splice(team2Index, 1);
        return;
      }
    }
  }

  broadcastToMatch(matchId, event, data) {
    // Implementation depends on io instance
    console.log(`Broadcasting ${event} to match ${matchId}`);
  }
}

module.exports = new GameService();
