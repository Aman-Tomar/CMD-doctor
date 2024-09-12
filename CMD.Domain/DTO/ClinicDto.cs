using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Domain.DTO
{
    public class ClinicDto
    {
        public int ClinicId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public ClinicAddressDto ClinicAddress { get; set; }
    }
}
