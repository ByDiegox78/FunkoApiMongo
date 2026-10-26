using FluentValidation;
using FunkoApiMongo.Dto;
using FunkoApiV._2.Infrastructure.Interfaces;
using MongoDB.Bson;

namespace FunkoApiMongo.Validator;

public class FunkoCreateValidator : AbstractValidator<FunkoCreateRequest>, ITransientService {
    public FunkoCreateValidator() {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(20);
        RuleFor(x => x.Precio).InclusiveBetween(0.01m, 50.0m);
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
        
        // Cambio clave: Validar ProveedorId comprobando que sea un ObjectId válido
        RuleFor(x => x.ProveedorId)
            .NotEmpty().WithMessage("El id proveedor es obligatorio")
            .Must(id => ObjectId.TryParse(id, out _)).WithMessage("Formato de ObjectId inválido");
    }
}