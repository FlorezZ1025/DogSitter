using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Application.Raza.Commands
{
    public record DeleteRazaCommand(Guid id) : IRequest;
}
