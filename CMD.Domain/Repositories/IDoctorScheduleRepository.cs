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
    /// This interface defines the contract for CRUD operations related to doctor schedules,
    /// such as creating, retrieving, updating, and retrieving schedules by doctor or weekday.
    /// </summary>
    public interface IDoctorScheduleRepository
    {
        /// <summary>
        /// Asynchronously adds a new doctor schedule to the system.
        /// </summary>
        /// <param name="doctorSchedule">The doctor schedule entity to be created, containing details such as clinic, weekday, start time, end time, and doctor ID.</param>
        /// <returns>
        /// A Task that represents the asynchronous operation of creating a doctor schedule.
        /// </returns>
        Task CreateDoctorSchedule(DoctorSchedule doctorSchedule);

        /// <summary>
        /// Asynchronously retrieves a list of doctor schedules for a given doctor on a specific weekday.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose schedule is being requested.</param>
        /// <param name="weekday">The day of the week (e.g., "Monday") for which the schedule is being requested.</param>
        /// <returns>
        /// A Task that represents the asynchronous operation, returning a list of <see cref="DoctorSchedule"/> entities 
        /// matching the specified doctor ID and weekday. If no schedules are found, the result will be an empty list.
        /// </returns>
        Task<List<DoctorSchedule>> GetDoctorScheduleForWeekday(int doctorId, string weekday);

        /// <summary>
        /// Asynchronously updates an existing doctor schedule.
        /// </summary>
        /// <param name="doctorSchedule">The doctor schedule entity containing updated details.</param>
        /// <returns>
        /// A Task that represents the asynchronous operation of updating the doctor schedule.
        /// </returns>
        Task UpdateDoctorSchedule(DoctorSchedule doctorSchedule);

        /// <summary>
        /// Asynchronously retrieves a doctor schedule by its unique identifier.
        /// </summary>
        /// <param name="doctorScheduleId">The unique identifier of the doctor schedule.</param>
        /// <returns>
        /// A Task that represents the asynchronous operation, returning the <see cref="DoctorSchedule"/> entity 
        /// if found; otherwise, null.
        /// </returns>
        Task<DoctorSchedule> GetDoctorScheduleById(int doctorScheduleId);

        /// <summary>
        /// Asynchronously retrieves a list of schedules for a specific doctor based on the provided doctor ID.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose schedules are to be retrieved.</param>
        /// <returns>
        /// A Task that represents the asynchronous operation. The task result contains a list of <see cref="DoctorSchedule"/> 
        /// objects associated with the doctor. If no schedules are found, the result will be an empty list.
        /// </returns>
        Task<List<DoctorSchedule>> GetScheduleByDoctorId(int doctorId);
    }
}
