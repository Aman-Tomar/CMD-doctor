using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Data.Context;
using CMD.Domain.Entities;
using CMD.Domain.Enums;
using CMD.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CMD.Data.Repositories
{
    /// <summary>
    /// Repository class for managing doctor schedules in the database.
    /// Implements the <see cref="IDoctorScheduleRepository"/> interface for handling CRUD operations related to doctor schedules.
    /// </summary>
    public class DoctorScheduleRepository : IDoctorScheduleRepository
    {
        private readonly DoctorDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorScheduleRepository"/> class with a database context.
        /// </summary>
        /// <param name="context">The <see cref="DoctorDbContext"/> instance for interacting with the database.</param>
        public DoctorScheduleRepository(DoctorDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Asynchronously adds a new doctor schedule to the database and saves the changes.
        /// </summary>
        /// <param name="doctorSchedule">The <see cref="DoctorSchedule"/> entity to be added to the database, containing the schedule details.</param>
        /// <returns>
        /// A task representing the asynchronous operation of adding the new doctor schedule.
        /// </returns>
        public async Task CreateDoctorSchedule(DoctorSchedule doctorSchedule)
        {
            _context.DoctorSchedules.Add(doctorSchedule);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously retrieves doctor schedules for a specific doctor on a given weekday.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose schedule is being retrieved.</param>
        /// <param name="weekday">The specific weekday (e.g., "Monday") for which the schedule is required.</param>
        /// <returns>
        /// A task representing the asynchronous operation, returning a list of <see cref="DoctorSchedule"/> entities.
        /// </returns>
        public async Task<List<DoctorSchedule>> GetDoctorScheduleForWeekday(int doctorId, Weekday weekday)
        {
            return await _context.DoctorSchedules
                                 .Where(s => s.DoctorId == doctorId && s.Weekday == weekday)
                                 .ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a doctor schedule by its unique identifier.
        /// </summary>
        /// <param name="doctorScheduleId">The unique identifier of the doctor schedule.</param>
        /// <returns>
        /// A task representing the asynchronous operation, returning the <see cref="DoctorSchedule"/> entity if found; otherwise, null.
        /// </returns>
        public async Task<DoctorSchedule> GetDoctorScheduleById(int doctorScheduleId)
        {
            return await _context.DoctorSchedules.FindAsync(doctorScheduleId);
        }

        /// <summary>
        /// Asynchronously updates an existing doctor schedule in the database.
        /// </summary>
        /// <param name="doctorSchedule">The <see cref="DoctorSchedule"/> entity containing updated details.</param>
        /// <returns>
        /// A task representing the asynchronous operation of updating the doctor schedule.
        /// </returns>
        public async Task UpdateDoctorSchedule(DoctorSchedule doctorSchedule)
        {
            _context.DoctorSchedules.Update(doctorSchedule);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a list of schedules associated with a specific doctor based on the provided doctor ID.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose schedules are to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="DoctorSchedule"/> objects associated with the doctor.
        /// </returns>
        public async Task<List<DoctorSchedule>> GetScheduleByDoctorId(int doctorId)
        {
            return await _context.DoctorSchedules
                                 .Where(s => s.DoctorId == doctorId)
                                 .ToListAsync();
        }
    }
}
