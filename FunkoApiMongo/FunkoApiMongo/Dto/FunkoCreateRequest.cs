using System.ComponentModel.DataAnnotations;

namespace FunkoApiMongo.Dto;

public record FunkoCreateRequest {
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(20, ErrorMessage = "El nombre solo puede tener 20 caracteres")]
    [MinLength(2, ErrorMessage = "El nombre debe tener 2 caracteres al menos")]
    public string Name { get; init; } = string.Empty;
        
    [Range(0.01, 50.0, ErrorMessage = "El precio debe estar entre 0.01 y 50.0")]
    public decimal Precio { get; init; }
    [Required(ErrorMessage = "El id proveedor es obligatorio")]
    public string ProveedorId { get; init; } 
    
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; init; }

    [Required(ErrorMessage = "La categoria es obligatoria")]
    public CategoriaDto Categoria { get; init; }
}

