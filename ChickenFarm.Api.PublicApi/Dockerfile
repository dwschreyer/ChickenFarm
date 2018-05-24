FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ChickenFarm.Api.PublicApi/ChickenFarm.Api.PublicApi.csproj ChickenFarm.Api.PublicApi/
COPY ChickenFarm/ChickenFarm.csproj ChickenFarm/
COPY ChickenFarm.GrainContracts/ChickenFarm.GrainContracts.csproj ChickenFarm.GrainContracts/
RUN dotnet restore ChickenFarm.Api.PublicApi/ChickenFarm.Api.PublicApi.csproj
COPY . .
WORKDIR /src/ChickenFarm.Api.PublicApi
RUN dotnet build ChickenFarm.Api.PublicApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ChickenFarm.Api.PublicApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ChickenFarm.Api.PublicApi.dll"]
