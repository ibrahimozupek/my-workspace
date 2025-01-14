# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GeziRehberi/GeziRehberi.csproj", "GeziRehberi/"]
RUN dotnet restore "./GeziRehberi/GeziRehberi.csproj"
COPY . .
WORKDIR "/src/GeziRehberi"
RUN dotnet build "./GeziRehberi.csproj" -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "./GeziRehberi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["appsettings.json", "/app/appsettings.json"]
ENTRYPOINT ["dotnet", "GeziRehberi.dll"]
