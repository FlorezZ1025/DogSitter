using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Application.Raza.Commands
{
    public record PatchRazaCommand(UpdateRazaDto dto) : IRequest<RazaDto>;
}
