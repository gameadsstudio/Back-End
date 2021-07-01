# ---- Build image ----
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build

WORKDIR /src

ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
ENV ASPNETCORE_ENVIRONMENT=Production

# Restore
COPY api.csproj .
RUN dotnet restore api.csproj

# Sources
COPY . .

# Build
RUN dotnet build api.csproj -c Release -o /app/build

# Publish
RUN dotnet publish api.csproj -c Release -o /app/publish

# ---- Runtime image ----
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine

WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

EXPOSE 5000

ENTRYPOINT ["dotnet", "api.dll"]
