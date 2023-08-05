#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Orders/EShop.Orders.API/EShop.Orders.API.csproj", "Services/Orders/EShop.Orders.API/"]
COPY ["BuildingBloks/Events/IntegrationEventLogEntityFramework/IntegrationEventLogEntityFramework.csproj", "BuildingBloks/Events/IntegrationEventLogEntityFramework/"]
COPY ["BuildingBloks/Events/EventBus/EventBus.csproj", "BuildingBloks/Events/EventBus/"]
COPY ["Services/Orders/EShop.Orders.Domain/EShop.Orders.Domain.csproj", "Services/Orders/EShop.Orders.Domain/"]
COPY ["Services/Orders/EShop.Orders.Infrastructure/EShop.Orders.Infrastructure.csproj", "Services/Orders/EShop.Orders.Infrastructure/"]
RUN dotnet restore "Services/Orders/EShop.Orders.API/EShop.Orders.API.csproj"
COPY . .
WORKDIR "/src/Services/Orders/EShop.Orders.API"
RUN dotnet build "EShop.Orders.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShop.Orders.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShop.Orders.API.dll"]