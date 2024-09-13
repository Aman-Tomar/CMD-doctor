using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Domain.Exceptions
{
    public class InvalidPhoneNumberException: ApplicationException
    {
        public InvalidPhoneNumberException(string message = null, Exception ex = null) : base(message, ex) { }

    }
}
