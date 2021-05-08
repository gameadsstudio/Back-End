# Game Ads Studio Api

## Prérequis

-   [Docker](https://docs.docker.com/get-docker/) pour pouvoir lancer le projet

## Lancement du projet

1. Copier le contenu du fichier .env.example dans un fichier .env et compléter les champs vides
2. Lancer le projet via docker
    - `$ docker-compose up --build`
    - ou
    - `$ docker-compose -f docker-compose.dev.yml up --build`

## Développement

La documentation de l'API est autogénérée par Swagger et se trouve sur `http://localhost:GAS_PORT/documentation`

### Notes
