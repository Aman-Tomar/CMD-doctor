using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.Entities;
using CMD.Domain.Entities;

namespace CMD.Domain.Repositories
{
    /// <summary>
    /// Interface for performing operations related to the Doctor entity.
    /// This defines the contract for doctor-related data access operations.
    /// The actual implementation will reside in the infrastructure layer.
    /// </summary>
    public interface IDoctorRepository
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync(int pageNumber,int pageSize);
        Task<int> GetTotalNumberOfDoctorsAsync();
        Task EditDoctor(Doctor doctor);
        Task<Doctor> GetDoctorById(int id);
        Task AddDoctorAsync(Doctor doctor);
    }
}
