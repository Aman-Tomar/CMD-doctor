using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Data.Context;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CMD.Data.Repostories
{
    /// <summary>
    /// Provides an implementation of the <see cref="IDoctorScheduleRepository"/> to interact with doctor schedules in the database.
    /// </summary>
    public class DoctorScheduleRepository : IDoctorScheduleRepository
    {
        private readonly DoctorDbContext _context;

        public DoctorScheduleRepository(DoctorDbContext context)
        {
            this._context = context;
        }

        /// <summary>
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
