using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMD.Domain.DTO;
using CMD.Domain.Entities;
using CMD.Domain.Enums;

namespace CMD.Domain.Managers
{
    public interface IDoctorScheduleManager
    {
        /// <summary>
        /// Checks if the specified doctor is available for a new schedule by verifying there are no overlapping schedules.
        /// </summary>
        /// <param name="doctorId">ID of the doctor.</param>
        /// <param name="weekday">Weekday for the schedule.</param>
        /// <param name="startTime">Start time of the schedule.</param>
        /// <param name="endTime">End time of the schedule.</param>
        /// <returns>A task representing the asynchronous operation, returning <see langword="true"/> if the doctor is available, otherwise <see langword="false"/>.</returns>
        Task<bool> IsDoctorAvailableForSchedule(int doctorId, Weekday weekday, string startTime, string endTime);

        /// <summary>
        /// Validates if the start time is earlier than the end time for a schedule.
        /// </summary>
        /// <param name="startTime">Start time of the schedule.</param>
        /// <param name="endTime">End time of the schedule.</param>
        /// <returns>A task representing the asynchronous operation, returning <see langword="true"/> if the time range is valid, otherwise <see langword="false"/>.</returns>
        Task<bool> IsScheduleTimeValid(string startTime, string endTime);

        /// <summary>
        /// Creates a new doctor schedule.
        /// </summary>
        /// <param name="doctorId">ID of the doctor.</param>
        /// <param name="doctorScheduleDto">Data Transfer Object containing the schedule details.</param>
        /// <returns>A task representing the asynchronous operation, returning the created <see cref="DoctorSchedule"/>.</returns>
        Task<DoctorSchedule> CreateDoctorSchedule(int doctorId, DoctorScheduleDto doctorScheduleDto);

        /// <summary>
        /// Edits an existing doctor schedule.
        /// </summary>
        /// <param name="doctorScheduleDto">Updated schedule details in a Data Transfer Object.</param>
        /// <param name="doctorSchedule">The existing <see cref="DoctorSchedule"/> entity to be updated.</param>
        /// <param name="doctor">The <see cref="Doctor"/> associated with the schedule.</param>
        /// <returns>A task representing the asynchronous operation, returning the updated <see cref="DoctorSchedule"/>.</returns>
        Task<DoctorSchedule> EditDoctorSchedule(DoctorScheduleDto doctorScheduleDto, DoctorSchedule doctorSchedule, Doctor doctor);

        /// <summary>
        /// Retrieves paginated doctor schedules.
        /// </summary>
        /// <param name="doctorSchedules">List of doctor schedules to paginate.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A task representing the asynchronous operation, returning a paginated result object.</returns>
        Task<Object> GetDoctorSchedule(List<DoctorSchedule> doctorSchedules, int page, int pageSize);
    }
}
