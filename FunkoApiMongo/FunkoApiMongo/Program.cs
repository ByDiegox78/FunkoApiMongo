using FunkoApiMongo.Cache;
using FunkoApiMongo.Config;
using FunkoApiMongo.Converter;
using FunkoApiMongo.Infrastructure;
using FunkoApiMongo.Models;
using FunkoApiMongo.Repository;
using FunkoApiV._2.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

// 1. Configuración tipada (lee las claves que ya tienes en los appsettings)
var config = new FunkoConfig {
    ConnectionString = builder.Configuration.GetConnectionString("MongoDb")
        ?? throw new InvalidOperationException("Falta ConnectionStrings:MongoDb"),
    DatabaseName = builder.Configuration["DatabaseSettings:Name"]
        ?? throw new InvalidOperationException("Falta DatabaseSettings:Name"),
    Name = builder.Configuration["ApiSettings:Name"] ?? "Funko API MongoDB",
    Cache = builder.Configuration["Cache:Name"] ?? "Funko API Cache"
};

var repositoryType = builder.Configuration["Funkos:RepositoryType"] ?? "MongoNative";

// 2. Controladores
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new ObjectIdJsonConverter());
    });

// 3. EF Core
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseMongoDB(config.ConnectionString, config.DatabaseName);
});

// 4. Driver nativo
builder.Services.AddSingleton<IMongoClient>(new MongoClient(config.ConnectionString));
builder.Services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(config.DatabaseName));
builder.Services.AddScoped(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Funko>("funkos"));

builder.Services.Configure<CacheConfig>(
    builder.Configuration.GetSection(CacheConfig.CacheName));

if (builder.Environment.IsDevelopment()) {
    builder.Services.AddMemoryCache();
    builder.Services.AddSingleton<ICacheService, MemCacheService>();
} else {
    builder.Services.AddStackExchangeRedisCache(options => {
        options.Configuration = builder.Configuration.GetConnectionString("Redis"); 
    });
    builder.Services.AddSingleton<ICacheService, RedisService>();
}

// 5. Scrutor
builder.Services.AddDependencies();

// 6. Repositorio según configuración
builder.Services.RemoveAll<IFunkoRepository>();

if (string.Equals(repositoryType, "MongoEF", StringComparison.OrdinalIgnoreCase))
    builder.Services.AddScoped<IFunkoRepository, FunkoEFRepository>();
else
    builder.Services.AddScoped<IFunkoRepository, FunkoNativeRepository>();

// 7. OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// 8. Cabecera
app.Logger.LogInformation("""
    ============================================================
                     CONFIGURACIÓN DE FUNKO API
    ============================================================
     API            : {Api}
     Entorno        : {Entorno}
     Mongo          : {Mongo}
     Base de datos  : {Database}
     Repositorio    : {Repositorio}
     Cache          : {Cache}
    ============================================================
    """,
    config.Name,
    app.Environment.EnvironmentName,
    config.ConnectionString,
    config.DatabaseName,
    repositoryType,
    config.Cache
    );

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    FunkoApiMongo.Seed.FunkoMongoDbSeeder.Seed(context);
}

app.MapControllers();
app.Run();