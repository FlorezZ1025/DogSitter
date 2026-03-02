using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Application.Perro.Commands
{
    public record DeletePerroCommand(Guid Id) : IRequest;
}
