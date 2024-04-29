FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8081
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./DataReplicationByKafka/DataReplicationByKafka.csproj ./DataReplicationByKafka/
COPY ./DAL/DAL.csproj ./DAL/
COPY ./Domain/Domain.csproj ./Domain/
COPY ./Service/Service.csproj ./Service/
RUN dotnet restore ./DataReplicationByKafka/DataReplicationByKafka.csproj

COPY . .
WORKDIR /src/DataReplicationByKafka
RUN dotnet build "DataReplicationByKafka.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataReplicationByKafka.csproj" -c Release -o /app/publish /p:UseAppHost=false
 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataReplicationByKafka.dll"]