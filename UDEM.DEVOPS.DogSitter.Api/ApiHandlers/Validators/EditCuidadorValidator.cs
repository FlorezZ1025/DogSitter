using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands;

namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers.Validators
{
    public class EditCuidadorValidator : AbstractValidator<EditCuidadorCommand>
    {
        public EditCuidadorValidator()
        {
            RuleFor(x => x.dto.Id).NotNull();
            RuleFor(x => x.dto.nombre).NotNull();
            RuleFor(x => x.dto.email).NotNull().EmailAddress();
            RuleFor(x => x.dto.telefono).NotNull();
            RuleFor(x => x.dto.direccion).NotNull();
            RuleFor(x => x.dto.activo).NotNull();
            RuleFor(x => x.dto.fechaInicioExperiencia).NotNull();
        }
    }
}
