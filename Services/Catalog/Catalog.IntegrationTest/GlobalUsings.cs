global using System.Net;
global using System.Text;
global using Xunit;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using DotNet.Testcontainers.Builders;
global using EShop.Catalog.API;
global using EShop.Catalog.API.Entities;
global using EShop.Catalog.API.Infrastructure;
global using IntegrationEventLogEntityFramework;
global using Testcontainers.MsSql;
global using Testcontainers.RabbitMq;
global using Newtonsoft.Json;
