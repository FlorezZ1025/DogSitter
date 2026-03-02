using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Domain.Exceptions
{
    public class DuplicatedEmailException : CoreBusinessException
    {
        public DuplicatedEmailException(string message) : base(message) { }

        public DuplicatedEmailException(string message, Exception inner) : base(message, inner) { }
    }
}
