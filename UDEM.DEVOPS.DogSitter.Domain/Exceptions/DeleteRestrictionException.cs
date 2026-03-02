using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Domain.Exceptions
{

    public class DeleteRestrictionException : CoreBusinessException
    {
        public DeleteRestrictionException(string message) : base(message) { }

        public DeleteRestrictionException(string message, Exception inner) : base(message, inner) { }
    }
}
