#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 2020:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApp/EShop.Web.MVC/EShop.Web.MVC.csproj", "WebApp/EShop.Web.MVC/"]
RUN dotnet restore "WebApp/EShop.Web.MVC/EShop.Web.MVC.csproj"
COPY . .
WORKDIR "/src/WebApp/EShop.Web.MVC"
RUN dotnet build "EShop.Web.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShop.Web.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShop.Web.MVC.dll"]