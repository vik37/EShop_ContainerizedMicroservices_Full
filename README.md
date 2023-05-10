
# EShop_ContainerizedMicroservices_Full

This project is about an E-Shop application, developed in microservices architecture. Includes new technologies in which I had no previous knowledge or less like GRPC, WebSocket, No-SQL DB (Redis), API Versioning, Polly (Retry), testing, implementing SQRS, DDD, architecture, 
Event Sourcing approach and also containerized with Docker, Docker composition with YML file. I'm reading the book .NET Microservice: Architecture for containerized .Net Applications from Microsoft and trying to implement the code which is partially written in the book itself, and more is explained about the subject or topic, and the rest you have to implement yourself. A good book.


## 🚀 About Me
I'm a Viktor Zafirovski - Web Developer and I work with technologies: Dot Net Core, SSMS, Angular, and programming languages: C#, SQL, and TypeScript(less).
Building Dot Net MVC application, Dot Net Web API (RESTFul App), Angular SPA. 


## Badges



[![GitHub repo file count](https://img.shields.io/github/directory-file-count/vik37/EShop_ContainerizedMicroservices_Full?color=yellow&logoColor=green&style=plastic)](https://img.shields.io/github/directory-file-count/vik37/EShop_ContainerizedMicroservices_Full?color=yellow&logoColor=green&style=plastic)

[![GitHub commit activity](https://img.shields.io/github/commit-activity/m/vik37/EShop_ContainerizedMicroservices_Full)](https://img.shields.io/github/commit-activity/m/vik37/EShop_ContainerizedMicroservices_Full)

![GitHub issues](https://img.shields.io/github/issues/vik37/EShop_ContainerizedMicroservices_Full)

[![Nuget](https://img.shields.io/nuget/v/Swashbuckle.AspNetCore)](https://img.shields.io/nuget/v/Swashbuckle.AspNetCore)

[![Twitter URL](https://img.shields.io/twitter/url?style=social&url=https%3A%2F%2Ftwitter.com%2FViktorZafirovs1)](https://img.shields.io/twitter/url?style=social&url=https%3A%2F%2Ftwitter.com%2FViktorZafirovs1)

## Deployment

To deploy this project run

```cmd
  docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```


## 🔗 Links

[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/viktor-zafirovski-8165725a/)

[![twitter](https://img.shields.io/badge/twitter-1DA1F2?style=for-the-badge&logo=twitter&logoColor=white)](https://twitter.com/)


## Usage/Examples

```csharp
public static IServiceCollection DatabaseConfiguration(this IServiceCollection services, IConfiguration config)=>
        services.AddDbContext<CatalogDbContext>(opt =>
        {
            if (config != null)
            {
                opt.UseSqlServer(Application.GetApplication().DockerMSQLConnectionString(config),
                sqlServerOptionsAction: sqlOption =>
                {
                    sqlOption.MigrationsAssembly(
                            Assembly.GetExecutingAssembly().GetName().Name
                        );
                    sqlOption.EnableRetryOnFailure(maxRetryCount: 5,
                                                    maxRetryDelay: TimeSpan.FromSeconds(30),
                                                    errorNumbersToAdd: null);
                });
            }
        });
```
- Retry 5 times with the Microsoft SQL database on docker image mcr.microsoft.com/mssql/server:2022-latest while the connection is established.
For retry, there is a package "Polly".

## Tech Stack

**Client:** DotNet MVC 6.0  

**Server:** Dot Net Core 6.0 Web API

**Databases:**
- **Catalog API Service:** Microsoft SQL, ORM - **Entity Framework**
- **Basket API Service:** No-SQL db - **Redis**

**Docker:** OS - Linux


## Run Locally

Clone the project

```bash
  git clone https://github.com/vik37/EShop_ContainerizedMicroservices_Full.git
```

Go to the project directory

```bash
  cd EShop_ContainerizedMicroservices_Full
```

build the solution

```bash
  dotnet build
```

run the app

```bash
  dotnet run
```
- To work locally just change the connection in the database configuration, the method:

**From:**
```
opt.UseSqlServer(Application.GetApplication().DockerMSQLConnectionString(config)
```
**To:**
```
opt.UseSqlServer(Application.GetApplication().LocalMSQLConnectionString(config)
```
```diff
+ ( ## This code is in the )
@@    EShop.Catalog.API     @@
```

## Other Common Github Profile Sections
👩‍💻 It is currently being worked on.

🧠


