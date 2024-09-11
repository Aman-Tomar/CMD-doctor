using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.Entities;

namespace CMD.Domain.Repositories
{
    public interface IDoctorScheduleRepository
    {
        /// <summary>
        /// Adds a new doctor schedule to the system.
        /// </summary>
        /// <param name="doctorSchedule">The doctor schedule entity to be created, containing details such as clinic, weekday, start time, end time, and doctor ID.</param>
        /// <returns>A task that represents the asynchronous operation of creating a doctor schedule.</returns>
        Task CreateDoctorSchedule(DoctorSchedule doctorSchedule);
        /// <summary>
        /// Retrieves a list of doctor schedules for a given doctor on a specific weekday.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose schedule is being requested.</param>
        /// <param name="weekday">The day of the week (e.g., "Monday") for which the schedule is being requested.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, returning a list of 
        /// <see cref="DoctorSchedule"/> entities matching the specified doctor ID and weekday.
        /// </returns>
        Task<List<DoctorSchedule>> GetDoctorScheduleForWeekday(int doctorId, string weekday);
    }
}
