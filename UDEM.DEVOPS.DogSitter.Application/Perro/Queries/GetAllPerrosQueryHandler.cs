using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Application.Perro.Queries
{
    public class GetAllPerrosQueryHandler(IPerroRepository perroRepository) : IRequestHandler<GetAllPerrosQuery, IEnumerable<PerroDto>>
    {
        const string TRAZA = "Se obtuvieron todas los perros";
        public async Task<IEnumerable<PerroDto>> Handle(GetAllPerrosQuery request, CancellationToken cancellationToken)
        {
            var perros = await perroRepository.GetAllAsync();
            throw new NotImplementedException();
        }
    }
}
