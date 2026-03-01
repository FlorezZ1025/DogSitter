using FluentValidation;
using UDEM.DEVOPS.DogSitter.Application.Raza.Commands;

namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers.Validators
{
    public class EditRazaValidator : AbstractValidator<EditRazaCommand>
    {
        public EditRazaValidator()
        {
            RuleFor(x => x.dto.Id).NotEmpty().NotNull();
            RuleFor(x => x.dto.nombre).NotEmpty().NotNull();
            RuleFor(x => x.dto.corpulencia).NotEmpty().NotNull();
            RuleFor(x => x.dto.observacionesGenerales).NotEmpty().NotNull();
            RuleFor(x => x.dto.nivelEnergia).NotEmpty().NotNull();
        }
    }
}
