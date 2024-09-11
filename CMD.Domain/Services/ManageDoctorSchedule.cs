using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using Microsoft.VisualBasic;

namespace CMD.Domain.Services
{
    /// <summary>
    /// Provides services for managing doctor schedules, including checking for availability.
    /// </summary>
    public class ManageDoctorSchedule : IManageDoctorSchedule
    {
        private readonly IDoctorScheduleRepository _doctorScheduleRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="ManageDoctorSchedule"/> class with the specified repository.
        /// </summary>
        /// <param name="doctorScheduleRepository">The repository for interacting with doctor schedules.</param>
        public ManageDoctorSchedule(IDoctorScheduleRepository doctorScheduleRepository)
        {
            this._doctorScheduleRepository = doctorScheduleRepository;
        }

        /// <summary>
        /// Checks if the specified doctor schedule is available by verifying there are no overlapping schedules.
        /// </summary>
        /// <param name="doctorSchedule">The doctor schedule to be checked, containing the doctor's ID, weekday, start time, and end time.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result is <see langword="true"/> if the schedule does not overlap with any existing schedules; otherwise, <see langword="false"/>.
        /// </returns>
        public async Task<bool> IsAvailable(DoctorSchedule doctorSchedule)
        {
            var existingSchedules = await _doctorScheduleRepository.GetDoctorScheduleForWeekday(doctorSchedule.DoctorId, doctorSchedule.Weekday);
            
            foreach (var schedule in existingSchedules)
            {
                if ((doctorSchedule.StartTime > schedule.StartTime && doctorSchedule.StartTime < schedule.EndTime) ||
                    (doctorSchedule.EndTime > schedule.StartTime && doctorSchedule.EndTime < schedule.EndTime) ||
                    (doctorSchedule.StartTime <= schedule.StartTime && doctorSchedule.EndTime >= schedule.EndTime))
                {
                    return false; // Overlapping schedules found
                }
            }

            return true;
        }
    }
}
