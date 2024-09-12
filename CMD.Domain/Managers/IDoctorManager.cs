using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMD.Domain.DTO;
using CMD.Domain.Entities;

namespace CMD.Domain.Managers
{
    /// <summary>
    /// Defines the contract for managing doctor-related operations.
    /// </summary>
    public interface IDoctorManager
    {
        /// <summary>
        /// Adds a new doctor to the system.
        /// </summary>
        /// <param name="doctorDto">The <see cref="DoctorDto"/> containing the details of the doctor to be added.</param>
        /// <returns>
        /// A <see cref="Task{Doctor}"/> representing the asynchronous operation, with a <see cref="Doctor"/> object representing the newly added doctor.
        /// </returns>
        Task<Doctor> AddDoctor(DoctorDto doctorDto);

        /// <summary>
        /// Edits an existing doctor in the system.
        /// </summary>
        /// <param name="doctor">The <see cref="Doctor"/> entity to be updated.</param>
        /// <param name="doctorDto">The <see cref="DoctorDto"/> containing the updated details of the doctor.</param>
        /// <returns>
        /// A <see cref="Task{Doctor}"/> representing the asynchronous operation, with the updated <see cref="Doctor"/> entity.
        /// </returns>
        Task<Doctor> EditDoctor(Doctor doctor, DoctorDto doctorDto);

        /// <summary>
        /// Retrieves a paginated list of doctors.
        /// </summary>
        /// <param name="doctors">A list of <see cref="Doctor"/> entities to be paginated.</param>
        /// <param name="page">The page number to retrieve (must be greater than 0).</param>
        /// <param name="pageSize">The number of items per page (must be greater than 0).</param>
        /// <returns>
        /// A <see cref="Task{object}"/> representing the asynchronous operation, with an object containing the paginated list of doctors along with pagination metadata.
        /// </returns>
        Task<object> GetAllDoctor(List<Doctor> doctors, int page, int pageSize);
    }
}
