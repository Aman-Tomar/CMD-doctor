using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Domain.Exceptions
{
    public class InvalidDoctorAddressException : ApplicationException
    {
        public InvalidDoctorAddressException(string message = null, Exception ex = null) : base(message, ex) { }

    }
}
