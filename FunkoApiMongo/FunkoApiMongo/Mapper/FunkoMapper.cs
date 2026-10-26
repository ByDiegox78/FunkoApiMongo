using FunkoApiMongo.Dto;
using FunkoApiMongo.Models;
using MongoDB.Bson;

namespace FunkoApiMongo.Mapper;

public static class FunkoMapper {
    public static FunkoResponseDto ToDto(this Funko f) => new() {
        Id = f.Id.ToString(),
        Name = f.Name,
        Precio = f.Precio,
        Stock = f.Stock,
        ProveedorId = f.ProveedorId.ToString(),
        Categoria = f.Categoria.ToDto(),
        CreatedAt = f.CreatedAt,
        UpdatedAt = f.UpdatedAt
    };

    public static Funko ToModel(this FunkoCreateRequest dto) => new() {
        Name = dto.Name,
        Precio = dto.Precio,
        Stock = dto.Stock,
        Categoria = dto.Categoria.ToModel(),      
        ProveedorId = ObjectId.Parse(dto.ProveedorId),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = null,
        IsDeleted = false
    };

    public static Funko ToModel(this FunkoUpdateRequest dto) => new() {
        Name = dto.Name,
        Precio = dto.Precio,
        Stock = dto.Stock,
        Categoria = dto.Categoria.ToModel(),
        ProveedorId = ObjectId.Parse(dto.ProveedorId),
        UpdatedAt = DateTime.UtcNow
    };

    public static ProveedorDto ToDto(this Proveedor c) => new(
        c.Id.ToString(),
        c.Nombre
        );
    
    public static CategoriaDto ToDto(this Categoria c) => new() {
        Nombre = c.Nombre,
        Descripcion = c.Descripcion
    };
    public static Categoria ToModel(this CategoriaDto dto) => new() {
        Nombre = dto.Nombre,
        Descripcion = dto.Descripcion
    };
}