﻿global using System.Net;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.OpenApi.Models;
global using Microsoft.AspNetCore.Mvc.Versioning;
global using EShop.Basket.API;
global using EShop.Basket.API.Model;
global using EShop.Basket.API.Repository;
global using EShop.Basket.API.IntegrationEvents.Events;
global using EventBus.Abstractions;
global using EventBussRabbitMQ;
global using EShop.Basket.API.IntegrationEvents.EventHandling;
global using EventBus;
global using RabbitMQ.Client;
global using StackExchange.Redis;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Serialization;
global using Serilog;
global using Serilog.Context;
global using EventBus.Events;