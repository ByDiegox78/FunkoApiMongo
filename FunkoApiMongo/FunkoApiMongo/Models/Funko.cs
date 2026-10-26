using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FunkoApiMongo.Models;

public record Funko {
    [BsonId]
    [Key]
    public ObjectId Id { get; set; }
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    [BsonElement("precio")]
    public decimal Precio { get; set; }
    [BsonElement("stock")]
    public int Stock { get; set; }
    [BsonElement("proveedor_id")]
    public ObjectId ProveedorId { get; set; }
    [BsonElement("categoria")]
    public Categoria Categoria { get; set; } = null!;
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }
    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; }
}
public record Categoria {
    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;
    [BsonElement("descripcion")]
    public string? Descripcion { get; set; } = string.Empty;
}