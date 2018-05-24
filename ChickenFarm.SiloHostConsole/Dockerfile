FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY ChickenFarm.SiloHostConsole/ChickenFarm.SiloHostConsole.csproj ChickenFarm.SiloHostConsole/
COPY ChickenFarm.Grains/ChickenFarm.Grains.csproj ChickenFarm.Grains/
COPY ChickenFarm/ChickenFarm.csproj ChickenFarm/
COPY ChickenFarm.GrainContracts/ChickenFarm.GrainContracts.csproj ChickenFarm.GrainContracts/
RUN dotnet restore ChickenFarm.SiloHostConsole/ChickenFarm.SiloHostConsole.csproj
COPY . .
WORKDIR /src/ChickenFarm.SiloHostConsole
RUN dotnet build ChickenFarm.SiloHostConsole.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ChickenFarm.SiloHostConsole.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ChickenFarm.SiloHostConsole.dll"]
