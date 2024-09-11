using CMD.Data.Repostories;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorScheduleController : ControllerBase
    {
        private readonly IDoctorScheduleRepository _doctorScheduleRepository;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorScheduleController(IDoctorScheduleRepository doctorScheduleRepository, IDoctorRepository doctorRepository)
        {
            this._doctorScheduleRepository = doctorScheduleRepository;
            this._doctorRepository = doctorRepository;
        }
        // POST ../api/DoctorSchedule/Add - FE007
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddDoctorSchedule()
        {
            return Ok();
        }

        /// <summary>
        /// Updates an existing doctor schedule in the system.
        /// </summary>
        /// <remarks>
        /// This endpoint allows for updating an existing doctor schedule. The schedule can be updated with new details such as clinic location, weekday, start time, end time, and status.
        /// If the update is successful, the response will be a 200 OK status. 
        /// If the schedule ID is not found or if any of the data is invalid, an appropriate error response will be returned.
        /// </remarks>
        /// <param name="doctorScheduleId">The unique identifier of the doctor schedule to be updated.</param>
        /// <param name="doctorSchedule">The updated schedule information, including new details for clinic, weekday, start time, end time, status, and the doctor ID.</param>
        /// <returns>
        /// A 200 OK status code if the schedule is successfully updated.
        /// A 400 Bad Request status code if the input data is invalid or if an exception occurs during processing.
        /// A 404 Not Found status code if the schedule with the provided ID or the doctor with the provided ID does not exist.
        /// </returns>
        /// <response code="200">Returned when the doctor schedule is successfully updated.</response>
        /// <response code="400">Returned when the input data is invalid or when an error occurs during processing.</response>
        /// <response code="404">Returned when the doctor schedule with the specified ID or the doctor with the specified ID is not found.</response>
        /// PUT ../api/DoctorSchedule/id
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditDoctorSchedule([FromQuery] int doctorScheduleId, [FromBody] DoctorSchedule doctorSchedule)
        {
            var existingDoctorSchedule = await _doctorScheduleRepository.GetDoctorScheduleById(doctorScheduleId);
            if (existingDoctorSchedule == null)
            {
                return NotFound();
            }

            // Getting doctor for schedule
            var doctor = await _doctorRepository.GetDoctorById(doctorSchedule.DoctorId);
            if (doctor == null)
            {
                return NotFound("Doctor not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Checking if startTime is before endTime
            if (doctorSchedule.StartTime < doctorSchedule.EndTime)
            {
                return BadRequest("Please enter schedule time properly.");
            }


            // Mapping
            existingDoctorSchedule.Clinic = doctorSchedule.Clinic;
            existingDoctorSchedule.Weekday = doctorSchedule.Weekday;
            existingDoctorSchedule.StartTime = doctorSchedule.StartTime;
            existingDoctorSchedule.EndTime = doctorSchedule.EndTime;
            existingDoctorSchedule.Status = doctorSchedule.Status;
            existingDoctorSchedule.LastModifiedBy = doctorSchedule.LastModifiedBy;
            existingDoctorSchedule.LastModifiedDate = DateTime.Now;
            existingDoctorSchedule.DoctorId = doctorSchedule.DoctorId;
            existingDoctorSchedule.Doctor = doctor;

            try
            {
                await _doctorScheduleRepository.UpdateDoctorSchedule(existingDoctorSchedule);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET ../api/DoctorSchedule
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDoctorSchedule()
        {
            return Ok();
        }
    }
}
