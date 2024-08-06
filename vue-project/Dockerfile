# Verwende ein Node.js Image zum Bauen der Anwendung
FROM node:16 AS build

# Setze das Arbeitsverzeichnis
WORKDIR /app

# Kopiere package.json und installiere Abhängigkeiten
COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

# Verwende ein NGINX Image für das fertige Build
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

