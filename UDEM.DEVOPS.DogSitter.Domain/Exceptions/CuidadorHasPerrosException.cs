using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Domain.Exceptions
{

    public class CuidadorHasPerrosException : CoreBusinessException
    {
        public CuidadorHasPerrosException(string message) : base(message) { }

        public CuidadorHasPerrosException(string message, Exception inner) : base(message, inner) { }
    }
}
