namespace FunkoApiMongo.Dto;

public record CategoriaDto {
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; } = string.Empty;
}