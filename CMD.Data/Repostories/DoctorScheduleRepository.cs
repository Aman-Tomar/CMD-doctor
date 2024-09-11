using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Data.Context;
using CMD.Domain.Entities;
using CMD.Data.Context;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMD.Data.Repostories
{
    /// <summary>
    /// Repository class for managing doctor schedules in the database.
    /// Implements the <see cref="IDoctorScheduleRepository"/> interface for 
    /// handling CRUD operations related to doctor schedules.
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
        /// Adds a new doctor schedule to the database and saves the changes.
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
        /// Retrieves doctor schedules for a specific doctor on a given weekday.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose schedule is being retrieved.</param>
        /// <param name="weekday">The specific weekday (e.g., "Monday") for which the schedule is required.</param>
        /// <returns>
        /// A task representing the asynchronous operation, returning a list of <see cref="DoctorSchedule"/> entities.
        /// </returns>
        public async Task<List<DoctorSchedule>> GetDoctorScheduleForWeekday(int doctorId, string weekday)
        {
            return await _context.DoctorSchedules
                                 .Where(s => s.DoctorId == doctorId && s.Weekday == weekday).ToListAsync();
        }
        
        public async Task<DoctorSchedule> GetDoctorScheduleById(int doctorScheduleId)
        {
            return await _context.DoctorSchedules.FindAsync(doctorScheduleId);
        }

        public async Task UpdateDoctorSchedule(DoctorSchedule doctorSchedule)
        {
            _context.DoctorSchedules.Update(doctorSchedule);
            await _context.SaveChangesAsync();
        /// Retrieves a list of schedules associated with a specific doctor based on the provided doctor ID.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose schedules are to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="DoctorSchedule"/> 
        /// objects associated with the doctor. If no schedules are found, the result will be an empty list.
        /// </returns>
        public async Task<List<DoctorSchedule>> GetScheduleByDoctorId(int doctorId)
        {
            return await _context.DoctorSchedules.Where( s => s.DoctorId == doctorId).ToListAsync();
        }
    }
}
