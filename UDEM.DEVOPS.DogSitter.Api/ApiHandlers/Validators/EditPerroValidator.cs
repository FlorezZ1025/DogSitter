using FluentValidation;
using UDEM.DEVOPS.DogSitter.Application.Perro.Commands;

namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers.Validators
{
    public class EditPerroValidator : AbstractValidator<EditPerroCommand>
    {
        public EditPerroValidator() 
        {
            RuleFor(x => x.dto.Id).NotEmpty().NotNull();
            RuleFor(x => x.dto.nombre).NotEmpty().NotNull();
            RuleFor(x => x.dto.peso).NotEmpty().NotNull();
            RuleFor(x => x.dto.edad).NotEmpty().NotNull();
            RuleFor(x => x.dto.horarioComida).NotEmpty().NotNull();
            RuleFor(x => x.dto.tipoComida).NotEmpty().NotNull();
        }
    }
}
