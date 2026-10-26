using MongoDB.Bson;
using MongoDB.Driver;
using FunkoApiMongo.Models;
using FunkoApiV._2.Entity;

namespace FunkoApiMongo.Seed;

public static class FunkoMongoDbSeeder
{
    // IDs fijos para los proveedores, así los Funkos pueden referenciarlos sin romperse
    private static readonly ObjectId FunkoCorpId = ObjectId.Parse("64b000000000000000000001");
    private static readonly ObjectId AmazonId = ObjectId.Parse("64b000000000000000000002");
    private static readonly ObjectId GameId = ObjectId.Parse("64b000000000000000000003");
    private static readonly ObjectId EmpId = ObjectId.Parse("64b000000000000000000004");
    private static readonly ObjectId FnacId = ObjectId.Parse("64b000000000000000000005");

    /// <summary>
    /// Siembra usando el Driver Nativo de MongoDB
    /// </summary>
    public static void Seed(IMongoDatabase database)
    {
        var proveedoresCollection = database.GetCollection<Proveedor>("proveedors");
        var funkosCollection = database.GetCollection<Funko>("funkos");

        // 1) Referenciados primero
        var proveedores = BuildProveedores();
        proveedoresCollection.DeleteMany(FilterDefinition<Proveedor>.Empty);
        proveedoresCollection.InsertMany(proveedores);

        // 2) Compuestos después
        var funkos = BuildFunkos();
        funkosCollection.DeleteMany(FilterDefinition<Funko>.Empty);
        funkosCollection.InsertMany(funkos);
    }

    /// <summary>
    /// Siembra usando Entity Framework Core
    /// </summary>
    public static void Seed(AppDbContext context)
    {
        var proveedores = BuildProveedores();
        var funkos = BuildFunkos();

        // 1) Limpiamos las colecciones primero para evitar duplicados en desarrollo
        context.Proveedor.RemoveRange(context.Proveedor);
        context.Funkos.RemoveRange(context.Funkos);
        context.SaveChanges();

        // 2) Insertamos los nuevos
        context.Proveedor.AddRange(proveedores);
        context.Funkos.AddRange(funkos);
        context.SaveChanges();
        
    }

    private static Proveedor[] BuildProveedores() =>
    [
        new() { Id = FunkoCorpId, Nombre = "Funko Oficial", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsDeleted = false },
        new() { Id = AmazonId, Nombre = "Amazon", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsDeleted = false },
        new() { Id = GameId, Nombre = "Game", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsDeleted = false },
        new() { Id = EmpId, Nombre = "EMP Merchandising", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsDeleted = false },
        new() { Id = FnacId, Nombre = "FNAC", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsDeleted = false }
    ];

    private static readonly (string Nombre, string Descripcion)[] CategoriasSemilla =
    [
        ("Anime", "Figuras de animación japonesa"),
        ("Videojuegos", "Personajes de videojuegos"),
        ("Películas", "Personajes del cine y Hollywood"),
        ("Series", "Personajes de series de televisión"),
        ("Música", "Artistas y bandas musicales")
    ];

    private static Categoria NuevaCategoria(int indice) => new()
    {
        Nombre = CategoriasSemilla[indice].Nombre,
        Descripcion = CategoriasSemilla[indice].Descripcion
    };

    private static List<Funko> BuildFunkos() =>
    [
        new() {
            Id = ObjectId.GenerateNewId(), Name = "Hinata Shoyo", Precio = 14.50m, Stock = 8,
            Categoria = NuevaCategoria(0), ProveedorId = FunkoCorpId, CreatedAt = DateTime.UtcNow, IsDeleted = false
        },
        new() {
            Id = ObjectId.GenerateNewId(), Name = "Kirito", Precio = 15.99m, Stock = 10,
            Categoria = NuevaCategoria(0), ProveedorId = AmazonId, CreatedAt = DateTime.UtcNow, IsDeleted = false
        },
        new() {
            Id = ObjectId.GenerateNewId(), Name = "Geralt de Rivia", Precio = 17.50m, Stock = 12,
            Categoria = NuevaCategoria(1), ProveedorId = GameId, CreatedAt = DateTime.UtcNow, IsDeleted = false
        },
        new() {
            Id = ObjectId.GenerateNewId(), Name = "Master Chief", Precio = 19.99m, Stock = 5,
            Categoria = NuevaCategoria(1), ProveedorId = GameId, CreatedAt = DateTime.UtcNow, IsDeleted = false
        },
        new() {
            Id = ObjectId.GenerateNewId(), Name = "Darth Vader", Precio = 21.00m, Stock = 3,
            Categoria = NuevaCategoria(2), ProveedorId = FnacId, CreatedAt = DateTime.UtcNow, IsDeleted = false
        }
    ];
}