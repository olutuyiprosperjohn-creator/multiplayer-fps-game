const queue = [];
const matches = new Map();

const QUEUE_CHECK_INTERVAL = 2000;
const MIN_PLAYERS_PER_MATCH = 2;
const MAX_PLAYERS_PER_MATCH = parseInt(process.env.MAX_PLAYERS_PER_MATCH) || 8;

class MatchmakingService {
  constructor(io) {
    this.io = io;
    this.startQueueProcessor();
  }

  addPlayerToQueue(socket, playerData) {
    const player = {
      socketId: socket.id,
      socket: socket,
      ...playerData,
      joinedAt: Date.now()
    };
    queue.push(player);
    console.log(`Player ${socket.id} joined queue. Queue size: ${queue.length}`);
  }

  removePlayerFromQueue(socketId) {
    const index = queue.findIndex(p => p.socketId === socketId);
    if (index !== -1) {
      queue.splice(index, 1);
      console.log(`Player ${socketId} removed from queue. Queue size: ${queue.length}`);
    }
  }

  startQueueProcessor() {
    setInterval(() => {
      if (queue.length >= MIN_PLAYERS_PER_MATCH) {
        this.createMatch();
      }
    }, QUEUE_CHECK_INTERVAL);
  }

  createMatch() {
    const playersCount = Math.min(queue.length, MAX_PLAYERS_PER_MATCH);
    const matchPlayers = queue.splice(0, playersCount);
    
    const matchId = `match_${Date.now()}`;
    const match = {
      id: matchId,
      players: matchPlayers,
      startTime: Date.now(),
      gameState: this.initializeGameState(matchPlayers)
    };

    matches.set(matchId, match);

    // Create room and notify players
    matchPlayers.forEach(player => {
      player.socket.join(matchId);
      player.socket.emit('match_found', { matchId, match: match.gameState });
    });

    console.log(`Match created: ${matchId} with ${playersCount} players`);
  }

  initializeGameState(players) {
    const team1 = players.slice(0, Math.ceil(players.length / 2));
    const team2 = players.slice(Math.ceil(players.length / 2));

    return {
      teams: {
        team1: team1.map(p => ({ socketId: p.socketId, username: p.username, health: 100, kills: 0, deaths: 0 })),
        team2: team2.map(p => ({ socketId: p.socketId, username: p.username, health: 100, kills: 0, deaths: 0 }))
      },
      timeElapsed: 0,
      score: { team1: 0, team2: 0 }
    };
  }

  getMatch(matchId) {
    return matches.get(matchId);
  }

  updateMatchState(matchId, state) {
    const match = matches.get(matchId);
    if (match) {
      match.gameState = state;
    }
  }
}

module.exports = new MatchmakingService();
