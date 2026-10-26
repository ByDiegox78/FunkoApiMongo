using FluentValidation;
using FunkoApiMongo.Dto;

namespace FunkoApiMongo.Validator;

public class CategoriaDtoValidator : AbstractValidator<CategoriaDto> {
    public CategoriaDtoValidator() {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la categoría es obligatorio")
            .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres");
    }
}