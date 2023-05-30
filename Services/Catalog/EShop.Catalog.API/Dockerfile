#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Catalog/EShop.Catalog.API/EShop.Catalog.API.csproj", "Services/Catalog/EShop.Catalog.API/"]
COPY ["BuildingBloks/Events/EventBus.csproj","BuildingBloks/Events/EventBus/"]
COPY ["BuildingBloks/Events/EventBussRabbitMQ.csproj","BuildingBloks/Events/EventBussRabbitMQ/"]
COPY ["BuildingBloks/Events/IntegrationEventLogEntityFramework.csproj","BuildingBloks/Events/IntegrationEventLogEntityFramework/"]
RUN dotnet restore "Services/Catalog/EShop.Catalog.API/EShop.Catalog.API.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/EShop.Catalog.API"
RUN dotnet build "EShop.Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShop.Catalog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShop.Catalog.API.dll"]