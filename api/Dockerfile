# ---- Build image ----
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

WORKDIR /src

ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
ENV ASPNETCORE_ENVIRONMENT=Production

# Restore
COPY *.csproj ./api/
RUN dotnet restore ./api/*.csproj

# Sources
COPY . ./api/

# Build
RUN dotnet publish ./api/*.csproj -c Release -o /app

# ---- Runtime image ----
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
ENV ASPNETCORE_ENVIRONMENT=Production

# SOURCES
COPY --from=build /app .

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "api.dll"]