# Fase 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0.403 AS build
WORKDIR /src

COPY . .

WORKDIR /src/DotnetMSWorkshop
RUN dotnet restore --verbosity detailed

RUN dotnet publish -c Release -o /app/publish --no-restore

# Fase 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build ./app/publish .
RUN find /app/

EXPOSE 8080
EXPOSE 8443

ENTRYPOINT ["dotnet", "DotnetMSWorkshopAPI.dll"]