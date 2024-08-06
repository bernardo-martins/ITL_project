# Verwende ein offizielles .NET SDK Image zum Bauen der Anwendung
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Setze das Arbeitsverzeichnis
WORKDIR /src

# Kopiere und restore die Abhängigkeiten
COPY *.csproj ./
RUN dotnet restore

# Kopiere den Rest des Codes und baue die Anwendung
COPY . ./
RUN dotnet publish -c Release -o /app

# Verwende ein Runtime-Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Exponiere Port
EXPOSE 82

# Starte die Anwendung
ENTRYPOINT ["dotnet", "AnikatzeDbProject.dll"]

