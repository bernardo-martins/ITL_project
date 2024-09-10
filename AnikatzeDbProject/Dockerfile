# Use an official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /src

# Copy and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the code and build the application
COPY . ./
RUN dotnet publish -c Release -o /app

# Use a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Copy the init.sql file
COPY init.sql /docker-entrypoint-initdb.d/

# Install PostgreSQL client
RUN apt-get update && apt-get install -y postgresql-client

# Expose port
EXPOSE 82

# Health check to ensure the database is ready
HEALTHCHECK --interval=10s --timeout=5s --start-period=30s --retries=3 \
  CMD pg_isready -h db -U postgres || exit 1

# Start the application and execute the SQL script if the database is empty
ENTRYPOINT ["sh", "-c", "until pg_isready -h db -U postgres; do echo 'Waiting for database...'; sleep 2; done; \
PGPASSWORD=anikatze createdb -h db -U postgres anikatze_postgres || echo 'Database already exists'; \
PGPASSWORD=anikatze psql -h db -U postgres -d anikatze_postgres -f /docker-entrypoint-initdb.d/init.sql && dotnet AnikatzeDbProject.dll && dotnet run --project /src/MockDataGenerator.csproj"]