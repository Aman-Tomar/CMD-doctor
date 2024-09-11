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
    /// Provides an abstraction for accessing and managing doctor schedules.
    /// </summary>
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
        Task UpdateDoctorSchedule(DoctorSchedule doctorSchedule);
        Task<DoctorSchedule> GetDoctorScheduleById(int doctorScheduleId);
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
