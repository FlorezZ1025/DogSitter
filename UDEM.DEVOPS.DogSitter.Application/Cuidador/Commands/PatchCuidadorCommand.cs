using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands
{
    public record PatchCuidadorCommand(UpdateCuidadorDto dto) : IRequest<CuidadorDto>;
}
