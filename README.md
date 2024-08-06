# Anikatze Project

This project is a web application that includes a frontend, backend, and PostgreSQL database. The setup is managed using Docker Compose.
This is the first time I used docker in that context and the markdown language, so don't yell at me for mistakes.

## Prerequisites

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Setup

### 1. Clone the Repository

First, clone this repository to your local machine:
```sh
https://gitea.hopeless-cloud.xyz/ITL/ITL.git
cd ITL
git checkout feature/Anikatze
```

The default configuration should work, but if you need to change any settings, you can update the docker-compose.yml file and the connection strings in appsettings.json. (at least for my Mac it works, can't test it on your devices)


### 2. Use Docker Compose to build and run the containers:

```sh
docker-compose up --build
```

Once the services are up and running, you can access them at the following URLs:

    Frontend: http://localhost:5173
    Backend API (Swagger UI): http://localhost:5169



