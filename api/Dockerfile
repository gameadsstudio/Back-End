# ---- Build image ----
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

WORKDIR /src

ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
ENV ASPNETCORE_ENVIRONMENT=Production

# Restore
COPY *.csproj ./game_ads_studio_api/
RUN dotnet restore ./game_ads_studio_api/*.csproj

# Sources
COPY . ./game_ads_studio_api/

# Build
RUN dotnet publish ./game_ads_studio_api/*.csproj -c Release -o /app

# ---- Runtime image ----
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
ENV ASPNETCORE_ENVIRONMENT=Production

# SOURCES
COPY --from=build /app .

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "game_ads_studio_api.dll"]