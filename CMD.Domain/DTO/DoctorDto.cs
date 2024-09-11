using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Domain.DTO
{
    public class DoctorDto
    {
        //public byte[]? ProfilePicture { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Specialization { get; set; }
        public string City { get; set; }
    }
}
