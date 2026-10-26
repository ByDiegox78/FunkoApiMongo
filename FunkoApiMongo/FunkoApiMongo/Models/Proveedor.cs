using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FunkoApiMongo.Models;

public record Proveedor {
    [BsonId]
    [Key]
    public ObjectId Id { get; set; }
    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; }
}