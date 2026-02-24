using DGW.DogSitter.DogWalker.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGW.DogSitter.DogWalker.Application.Cuidador.Queries
{
    public record GetCuidadorQuery(Guid id) : IRequest<CuidadorDto> ;
}
