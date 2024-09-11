using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.DTO;
using CMD.Domain.Entities;

namespace CMD.Domain.Repositories
{
    public interface IDoctorRepository
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync(int pageNumber,int pageSize);
        Task<int> GetTotalNumberOfDoctorsAsync();
    }
}
