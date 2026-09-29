const express = require('express');
const http = require('http');
const socketIo = require('socket.io');
const cors = require('cors');
const bodyParser = require('body-parser');
require('dotenv').config();

const authRoutes = require('./routes/auth');
const playerRoutes = require('./routes/players');
const matchmakingService = require('./services/matchmakingService');
const gameService = require('./services/gameService');

const app = express();
const server = http.createServer(app);
const io = socketIo(server, {
  cors: {
    origin: '*',
    methods: ['GET', 'POST']
  }
});

// Middleware
app.use(cors());
app.use(bodyParser.json());

// Routes
app.use('/api/auth', authRoutes);
app.use('/api/players', playerRoutes);

app.get('/', (req, res) => {
  res.json({ message: 'Multiplayer FPS Server', version: '1.0.0' });
});

// Socket.IO Connection
io.on('connection', (socket) => {
  console.log('Player connected:', socket.id);

  socket.on('join_queue', (data) => {
    matchmakingService.addPlayerToQueue(socket, data);
  });

  socket.on('leave_queue', () => {
    matchmakingService.removePlayerFromQueue(socket.id);
  });

  socket.on('player_input', (data) => {
    gameService.handlePlayerInput(socket.id, data);
  });

  socket.on('fire_weapon', (data) => {
    gameService.handleWeaponFire(socket.id, data);
  });

  socket.on('disconnect', () => {
    console.log('Player disconnected:', socket.id);
    matchmakingService.removePlayerFromQueue(socket.id);
    gameService.removePlayer(socket.id);
  });
});

const PORT = process.env.PORT || 3000;
server.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
  console.log(`WebSocket server active on port ${PORT}`);
});

module.exports = { app, io };
