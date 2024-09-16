using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Data.Context;
using CMD.Domain.Entities;
using CMD.Domain.Entities;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using CMD.Domain.DTO;

namespace CMD.Data.Repostories
{
    /// <summary>
    /// Repository class for managing doctor entities in the database.
    /// Implements the <see cref="IDoctorRepository"/> interface.
    /// </summary>
    public class DoctorRespository : IDoctorRepository
    {
        private readonly DoctorDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorRespository"/> class.
        /// </summary>
        /// <param name="context">The database context used to interact with the database.</param>
        public DoctorRespository(DoctorDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a doctor by their unique identifier.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Doctor"/> entity.</returns>
        public async Task<Doctor> GetDoctorByIdAsync(int doctorId)
        {
            return await _context.Doctors
                                 .Include(d => d.DoctorAddress)
                                 .FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        }

        /// <summary>
        /// Retrieves all active doctors from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Doctor"/> entities.</returns>
        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
                        .Include(d => d.DoctorAddress)
                        .ToListAsync();
        }

        /// <summary>
        /// Updates an existing doctor entity in the database.
        /// </summary>
        /// <param name="doctor">The <see cref="Doctor"/> entity to be updated.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task EditDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds a new doctor entity to the database.
        /// </summary>
        /// <param name="doctor">The <see cref="Doctor"/> entity to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a list of doctors by the department ID.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Doctor"/> entities belonging to the specified department.</returns>
        public async Task<List<Doctor>> GetDoctorsByDepartmentIdAsync(int departmentId)
        {
            return await _context.Doctors
                                 .Where(d => d.DepartmentId == departmentId)
                                 .ToListAsync();
        }
    }
}
