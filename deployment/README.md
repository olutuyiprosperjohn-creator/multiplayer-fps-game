# Deployment Guide

## Docker Compose (Recommended for Development)

```bash
docker-compose up
```

This will start:
- PostgreSQL database on port 5432
- Node.js server on port 3000

## Docker Build

Build the server image:
```bash
docker build -t multiplayer-fps-server:latest -f deployment/Dockerfile server/
```

## Production Deployment

### AWS EC2
1. Launch an EC2 instance (t3.medium or larger)
2. Install Docker and Docker Compose
3. Clone repository
4. Set environment variables in `.env`
5. Run `docker-compose up -d`

### Heroku
1. Create Heroku app
2. Add PostgreSQL addon
3. Deploy with Procfile

### DigitalOcean App Platform
1. Connect GitHub repository
2. Configure services (Web service + Postgres database)
3. Deploy

## SSL/TLS

Use Nginx reverse proxy with Let's Encrypt for HTTPS:
```bash
docker-compose -f docker-compose.yml -f docker-compose.ssl.yml up
```
