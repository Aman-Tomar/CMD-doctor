using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.Entities;

namespace CMD.Domain.Repositories
{
    /// <summary>
    /// Provides an abstraction for accessing and managing doctor schedules.
    /// </summary>
    public interface IDoctorScheduleRepository
    {
        /// <summary>
        /// Retrieves a list of schedules for a specific doctor based on the provided doctor ID.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose schedules are to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="DoctorSchedule"/> 
        /// objects associated with the doctor. If no schedules are found, the result will be an empty list.
        /// </returns>
        Task<List<DoctorSchedule>> GetScheduleByDoctorId(int doctorId);
    }
}
