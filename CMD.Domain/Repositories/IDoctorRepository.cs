using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMD.Domain.DTO;
using CMD.Domain.Entities;

namespace CMD.Domain.Repositories
{
    /// <summary>
    /// Interface for performing operations related to the <see cref="Doctor"/> entity.
    /// This defines the contract for doctor-related data access operations.
    /// The actual implementation will reside in the infrastructure layer.
    /// </summary>
    public interface IDoctorRepository
    {
        /// <summary>
        /// Asynchronously retrieves a list of all doctors.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Doctor"/> entities.
        /// </returns>
        Task<List<Doctor>> GetAllDoctorsAsync();
        
        /// <summary>
        /// Asynchronously retrieves a doctor by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the doctor.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="Doctor"/> entity if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Doctor> GetDoctorByIdAsync(int id);
        
        /// <summary>
        /// Asynchronously updates the details of an existing doctor.
        /// </summary>
        /// <param name="doctor">The <see cref="Doctor"/> entity with updated details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task EditDoctorAsync(Doctor doctor);

        /// <summary>
        /// Asynchronously adds a new doctor to the repository.
        /// </summary>
        /// <param name="doctor">The <see cref="Doctor"/> entity to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddDoctorAsync(Doctor doctor);
    }
}
