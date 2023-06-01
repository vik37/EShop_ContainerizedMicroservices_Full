#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Basket/EShop.Basket.API/EShop.Basket.API.csproj", "Services/Basket/EShop.Basket.API/"]
COPY ["BuildingBloks/Events/EventBus/EventBus.csproj","BuildingBloks/Events/EventBus/"]
COPY ["BuildingBloks/Events/EventBussRabbitMQ/EventBussRabbitMQ.csproj","BuildingBloks/Events/EventBussRabbitMQ/"]
RUN dotnet restore "Services/Basket/EShop.Basket.API/EShop.Basket.API.csproj"
COPY . .
WORKDIR "/src/Services/Basket/EShop.Basket.API"
RUN dotnet build "EShop.Basket.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShop.Basket.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShop.Basket.API.dll"]