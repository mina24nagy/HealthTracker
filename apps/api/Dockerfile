# syntax=docker/dockerfile:1
# Build context: repository root  →  docker build -f apps/api/Dockerfile .

# ---- build ----------------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Repo-level settings that affect the .NET build (SDK pin, code style).
COPY global.json .editorconfig ./
COPY apps/api/ apps/api/

# NuGet cache mount keeps rebuilds fast without listing every project file by hand.
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish apps/api/src/HealthTracker.Api/HealthTracker.Api.csproj \
        --configuration Release \
        --output /app/publish \
        -p:UseAppHost=false

# ---- runtime --------------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Non-root user provided by the official image.
USER $APP_UID
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HealthTracker.Api.dll"]
