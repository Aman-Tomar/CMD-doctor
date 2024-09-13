using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Domain.Exceptions
{
    public class InvalidDoctorScheduleException : ApplicationException
    {
        public InvalidDoctorScheduleException(string message = null, Exception ex = null) : base(message, ex) { }

    }
}
