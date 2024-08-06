
# Verwende ein offizielles .NET SDK Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Setze das Arbeitsverzeichnis
WORKDIR /src

# Kopiere und restore die Abhängigkeiten für Anikatze.Application
# Ensure permissions for scripts
COPY ./Anikatze.Application/*.csproj ./Anikatze.Application/
RUN dotnet restore ./Anikatze.Application/Anikatze.Application.csproj

# Kopiere und restore die Abhängigkeiten für Anikatze.Webapi
COPY ./Anikatze.Webapi/*.csproj ./Anikatze.Webapi/
RUN dotnet restore ./Anikatze.Webapi/Anikatze.Webapi.csproj

# Kopiere den Rest des Codes und baue die Anwendungen
COPY . .
RUN dotnet publish ./Anikatze.Application/Anikatze.Application.csproj -c Release -o /app/Application
RUN dotnet publish ./Anikatze.Webapi/Anikatze.Webapi.csproj -c Release -o /app/Webapi

# Verwende ein Runtime-Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/Application ./Application
COPY --from=build /app/Webapi ./Webapi

# Exponiere Ports
EXPOSE 8080

# Starte die Anwendungen
ENTRYPOINT ["dotnet", "Webapi/Anikatze.Webapi.dll"]

