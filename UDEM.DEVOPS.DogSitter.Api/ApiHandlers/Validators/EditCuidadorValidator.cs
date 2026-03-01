using FluentValidation;
using UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands;

namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers.Validators
{
    public class EditCuidadorValidator : AbstractValidator<EditCuidadorCommand>
    {
        public EditCuidadorValidator()
        {
            RuleFor(x => x.dto.Id).NotEmpty().NotNull();
            RuleFor(x => x.dto.nombre).NotEmpty().NotNull();
            RuleFor(x => x.dto.email).NotEmpty().NotNull().EmailAddress();
            RuleFor(x => x.dto.telefono).NotEmpty().NotNull();
            RuleFor(x => x.dto.direccion).NotEmpty().NotNull();
            RuleFor(x => x.dto.activo).NotEmpty().NotNull();
            RuleFor(x => x.dto.fechaInicioExperiencia).NotEmpty().NotNull();
        }
    }
}
