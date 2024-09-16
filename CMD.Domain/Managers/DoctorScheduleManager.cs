using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMD.Domain.DTO;
using CMD.Domain.Entities;
using CMD.Domain.Enums;
using CMD.Domain.Exceptions;
using CMD.Domain.Repositories;
using CMD.Domain.Services;

namespace CMD.Domain.Managers
{
    /// <summary>
    /// Provides services for managing doctor schedules, including creation, updates, validation, and pagination.
    /// </summary>
    public class DoctorScheduleManager : IDoctorScheduleManager
    {
        private readonly IDoctorScheduleRepository _doctorScheduleRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMessageService _messageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorScheduleManager"/> class.
        /// </summary>
        /// <param name="doctorScheduleRepository">Repository for interacting with doctor schedules.</param>
        /// <param name="doctorRepository">Repository for interacting with doctor information.</param>
        /// <param name="messageService">The service for providing custom error messages.</param>
        public DoctorScheduleManager(IDoctorScheduleRepository doctorScheduleRepository, IDoctorRepository doctorRepository, IMessageService messageService)
        {
            _doctorScheduleRepository = doctorScheduleRepository;
            _doctorRepository = doctorRepository;
            this._messageService = messageService;
        }

        /// <summary>
        /// Creates a new doctor schedule.
        /// </summary>
        /// <param name="doctorId">Doctor ID for whom the schedule is being created.</param>
        /// <param name="doctorScheduleDto">Data Transfer Object containing schedule details.</param>
        /// <returns>Created doctor schedule entity.</returns>
        public async Task<DoctorSchedule> CreateDoctorScheduleAsync(int doctorId, DoctorScheduleDto doctorScheduleDto)
        {
            // Validate weekday
            if (!Enum.TryParse<Weekday>(doctorScheduleDto.Weekday.ToUpper(), out var weekday))
            {
                throw new InvalidWeekdayException(_messageService.GetMessage("InvalidWeekdayException"));
            }

            // Validate schedule time
            if (await IsScheduleTimeValidAsync(doctorScheduleDto.StartTime, doctorScheduleDto.EndTime))
            {
                throw new InvalidTimeException(_messageService.GetMessage("InvalidTimeException"));
            }

            // Check if doctor is available for the schedule
            if (doctorScheduleDto.Status && !await IsDoctorAvailableForScheduleAsync(doctorScheduleDto.DoctorId, weekday, doctorScheduleDto.StartTime, doctorScheduleDto.EndTime))
            {
                throw new InvalidDoctorScheduleException(_messageService.GetMessage("InvalidDoctorScheduleException"));
            }


            // Map DTO to entity and save to the database
            var doctorSchedule = new DoctorSchedule
            {
                ClinicId = doctorScheduleDto.ClinicId,
                Weekday = weekday,
                StartTime = TimeOnly.Parse(doctorScheduleDto.StartTime),
                EndTime = TimeOnly.Parse(doctorScheduleDto.EndTime),
                Status = doctorScheduleDto.Status,
                DoctorId = doctorId,
                CreatedDate = DateTime.Now,
                CreatedBy = "admin",
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = "admin"
            };

            await _doctorScheduleRepository.CreateDoctorScheduleAsync(doctorSchedule);
            return doctorSchedule;
        }

        /// <summary>
        /// Edits an existing doctor schedule.
        /// </summary>
        /// <param name="doctorScheduleDto">Updated schedule details.</param>
        /// <param name="doctorSchedule">Existing schedule to be updated.</param>
        /// <param name="doctor">Doctor associated with the schedule.</param>
        /// <returns>Updated doctor schedule entity.</returns>
        public async Task<DoctorSchedule> EditDoctorScheduleAsync(DoctorScheduleDto doctorScheduleDto, DoctorSchedule doctorSchedule, Doctor doctor)
        {
            // Validate weekday
            if (!Enum.TryParse<Weekday>(doctorScheduleDto.Weekday.ToUpper(), out var weekday))
            {
                throw new InvalidWeekdayException(_messageService.GetMessage("InvalidWeekdayException"));
            }

            // Validate schedule time
            if (await IsScheduleTimeValidAsync(doctorScheduleDto.StartTime, doctorScheduleDto.EndTime))
            {
                throw new InvalidTimeException(_messageService.GetMessage("InvalidTimeException"));
            }

            // Check if doctor is available for the schedule
            if (doctorScheduleDto.Status && !await IsDoctorAvailableForScheduleAsync(doctorScheduleDto.DoctorId, weekday, doctorScheduleDto.StartTime, doctorScheduleDto.EndTime))
            {
                throw new InvalidDoctorScheduleException(_messageService.GetMessage("InvalidDoctorScheduleException"));
            }


            // Update the schedule entity
            doctorSchedule.ClinicId = doctorScheduleDto.ClinicId;
            doctorSchedule.Weekday = weekday;
            doctorSchedule.StartTime = TimeOnly.Parse(doctorScheduleDto.StartTime);
            doctorSchedule.EndTime = TimeOnly.Parse(doctorScheduleDto.EndTime);
            doctorSchedule.Status = doctorScheduleDto.Status;
            doctorSchedule.LastModifiedBy = "admin";
            doctorSchedule.LastModifiedDate = DateTime.Now;
            doctorSchedule.DoctorId = doctorScheduleDto.DoctorId;
            doctorSchedule.Doctor = doctor;

            await _doctorScheduleRepository.UpdateDoctorScheduleAsync(doctorSchedule);
            return doctorSchedule;
        }

        /// <summary>
        /// Retrieves doctor schedules with pagination support.
        /// </summary>
        /// <param name="doctorSchedules">List of doctor schedules to be paginated.</param>
        /// <param name="page">Current page number.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Paginated result containing doctor schedules for the specified page and size.</returns>
        public async Task<object> GetDoctorScheduleAsync(List<DoctorSchedule> doctorSchedules, int page, int pageSize)
        {
            if (page <= 0) throw new InvalidPageNumberException(_messageService.GetMessage("InvalidPageNumberException"));
            if (pageSize <= 0) throw new InvalidPageSizeException(_messageService.GetMessage("InvalidPageSizeException"));

            var schedules = doctorSchedules.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = doctorSchedules.Count(),
                TotalPages = (int)Math.Ceiling((double)doctorSchedules.Count() / pageSize),
                Data = schedules
            };
        }

        /// <summary>
        /// Checks if the doctor is available for the given schedule.
        /// </summary>
        /// <param name="doctorId">Doctor ID.</param>
        /// <param name="weekday">Weekday for the schedule.</param>
        /// <param name="startTime">Start time of the schedule.</param>
        /// <param name="endTime">End time of the schedule.</param>
        /// <returns>True if no overlapping schedules, false otherwise.</returns>
        public async Task<bool> IsDoctorAvailableForScheduleAsync(int doctorId, Weekday weekday, string startTime, string endTime)
        {
            var existingSchedules = await _doctorScheduleRepository.GetDoctorScheduleForWeekdayAsync(doctorId, weekday);
            TimeOnly sTime = TimeOnly.Parse(startTime);
            TimeOnly eTime = TimeOnly.Parse(endTime);

            return !existingSchedules.Any(schedule => (sTime > schedule.StartTime && sTime < schedule.EndTime) ||
                                                      (eTime > schedule.StartTime && eTime < schedule.EndTime) ||
                                                      (sTime <= schedule.StartTime && eTime >= schedule.EndTime));
        }

        /// <summary>
        /// Validates if the schedule's start time is earlier than the end time.
        /// </summary>
        /// <param name="startTime">Start time.</param>
        /// <param name="endTime">End time.</param>
        /// <returns>True if the time range is valid, false otherwise.</returns>
        public async Task<bool> IsScheduleTimeValidAsync(string startTime, string endTime)
        {
            if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime)) return false;
            return TimeOnly.Parse(startTime) >= TimeOnly.Parse(endTime);
        }
    }
}
