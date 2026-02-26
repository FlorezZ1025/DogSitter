using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands
{
    public record RegisterCuidadorCommand(CreateCuidadorDto dto) : IRequest<Guid>;
}
