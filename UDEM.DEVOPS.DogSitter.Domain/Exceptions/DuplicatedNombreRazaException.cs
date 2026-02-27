using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Domain.Exceptions
{
    public class DuplicatedNombreRazaException : CoreBusinessException
    {
        public DuplicatedNombreRazaException(string message) : base(message) { }

        public DuplicatedNombreRazaException(string message, Exception inner) : base(message, inner) { }
    }
}
