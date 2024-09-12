using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.DTO;

namespace CMD.Domain.Services
{
    public interface IClinicRepository
    {
        Task<bool> IsValidClinic(int clinicId);
        Task<ClinicAddressDto> GetClinicAddress(int clinicId);
    }
}
