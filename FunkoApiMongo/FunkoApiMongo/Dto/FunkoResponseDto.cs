namespace FunkoApiMongo.Dto;

public record FunkoResponseDto {
    public string Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Precio { get; init; }
    public int Stock { get; init; }
    public CategoriaDto Categoria { get; init; }
    public string ProveedorId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}