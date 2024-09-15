using CMD.Domain.DTO;
using CMD.Data.Repostories;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CMD.Domain.Managers;
using CMD.Domain.Services;

namespace CMD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorScheduleController : ControllerBase
    {
        private readonly IDoctorScheduleRepository _doctorScheduleRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorScheduleManager _doctorScheduleManager;
        private readonly IMessageService _messageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorScheduleController"/> class.
        /// </summary>
        /// <param name="doctorScheduleRepository">The repository for interacting with doctor schedule data.</param>
        /// <param name="doctorRepository">The repository for interacting with doctor data.</param>
        /// <param name="doctorScheduleManager">The manager for handling doctor schedule-related business logic.</param>
        /// <param name="messageService">The service for providing custom error messages.</param>
        public DoctorScheduleController(IDoctorScheduleRepository doctorScheduleRepository, IDoctorRepository doctorRepository, IDoctorScheduleManager doctorScheduleManager, IMessageService messageService)
        {
            this._doctorScheduleRepository = doctorScheduleRepository;
            this._doctorRepository = doctorRepository;
            this._doctorScheduleManager = doctorScheduleManager;
            this._messageService = messageService;
        }

        /// <summary>
        /// Creates a new doctor schedule.
        /// </summary>
        /// <remarks>
        /// This endpoint allows the creation of a doctor schedule by providing the necessary details such as available time slots, dates, clinic locations, and services offered. The request must include a valid JSON payload representing the doctor's schedule. If the schedule is created successfully, the response will be a 201 Created status. If the request contains invalid data or an error occurs during processing, a corresponding error response will be returned.
        /// </remarks>
        /// <param name="doctorId">The unique identifier of the doctor for whom the schedule is being created.</param>
        /// <param name="doctorScheduleDto">The <see cref="DoctorScheduleDto"/> containing the details of the schedule to be created.</param>
        /// <returns>
        /// A 201 Created status code if the schedule is successfully created.
        /// A 400 Bad Request status code if the input data is invalid or if an exception occurs during processing.
        /// </returns>
        /// <response code="201">Returned when the doctor schedule is successfully created.</response>
        /// <response code="400">Returned when the input data is invalid or when an error occurs during processing.</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDoctorSchedule([FromQuery] int doctorId, [FromBody] DoctorScheduleDto doctorScheduleDto)
        {
            // Check if all the properties are valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if doctor exists
            var doctor = await _doctorRepository.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                return NotFound(_messageService.GetMessage("DoctorNotFound"));
            }

            try
            {
                var doctorSchedule = await _doctorScheduleManager.CreateDoctorScheduleAsync(doctorId, doctorScheduleDto);
                var locationUri = $"api/DoctorSchedule/{doctorSchedule.DoctorScheduleId}";
                return Created(locationUri, doctorSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing doctor schedule.
        /// </summary>
        /// <remarks>
        /// This endpoint allows for updating an existing doctor schedule with new details such as clinic location, weekday, start time, end time, and status. If the update is successful, the response will be a 200 OK status. If the schedule ID or doctor ID is not found, or if any of the data is invalid, an appropriate error response will be returned.
        /// </remarks>
        /// <param name="doctorScheduleId">The unique identifier of the doctor schedule to be updated.</param>
        /// <param name="doctorScheduleDto">The <see cref="DoctorScheduleDto"/> containing the updated details for the doctor schedule.</param>
        /// <returns>
        /// A 200 OK status code if the schedule is successfully updated.
        /// A 400 Bad Request status code if the input data is invalid or if an exception occurs during processing.
        /// A 404 Not Found status code if the schedule or doctor with the provided ID does not exist.
        /// </returns>
        /// <response code="200">Returned when the doctor schedule is successfully updated.</response>
        /// <response code="400">Returned when the input data is invalid or when an error occurs during processing.</response>
        /// <response code="404">Returned when the doctor schedule with the specified ID or the doctor with the specified ID is not found.</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditDoctorSchedule([FromQuery] int doctorScheduleId, [FromBody] DoctorScheduleDto doctorScheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if doctor exists
            var doctor = await _doctorRepository.GetDoctorByIdAsync(doctorScheduleDto.DoctorId);
            if (doctor == null)
            {
                return NotFound(_messageService.GetMessage("DoctorNotFound"));
            }

            // Check if doctor schedule exists
            var existingDoctorSchedule = await _doctorScheduleRepository.GetDoctorScheduleByIdAsync(doctorScheduleId);
            if (existingDoctorSchedule == null)
            {
                return NotFound(_messageService.GetMessage("DoctorScheduleNotFound"));
            }

            try
            {
                var doctorSchedule = await _doctorScheduleManager.EditDoctorScheduleAsync(doctorScheduleDto, existingDoctorSchedule, doctor);
                return Ok(doctorSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the schedule for a specific doctor.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches the schedule associated with a doctor based on the provided doctor ID. If the doctor exists and has a schedule, it returns the schedule details. If the doctor does not exist or if no schedule is available for the doctor, an appropriate error response is returned.
        /// </remarks>
        /// <param name="doctorId">The unique identifier of the doctor whose schedule is being retrieved.</param>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <returns>
        /// A 200 OK status code with the doctor's schedule if found.
        /// A 404 Not Found status code if the doctor does not exist or if the doctor has no schedule.
        /// </returns>
        /// <response code="200">Returned when the doctor's schedule is successfully retrieved.</response>
        /// <response code="404">Returned when the doctor with the specified ID is not found or if no schedule is available for the doctor.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDoctorSchedule([FromQuery] int doctorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Check if doctor exists
            var doctor = await _doctorRepository.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                return NotFound(_messageService.GetMessage("DoctorNotFound"));
            }

            // Check if doctor has a schedule
            var doctorSchedules = await _doctorScheduleRepository.GetScheduleByDoctorIdAsync(doctorId);

            if (doctorSchedules == null || doctorSchedules.Count == 0)
            {
                return NotFound(_messageService.GetMessage("DoctorScheduleNotFound"));
            }

            try
            {
                var doctorSchedule = await _doctorScheduleManager.GetDoctorScheduleAsync(doctorSchedules, page, pageSize);
                return Ok(doctorSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{doctorScheduleId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetScheduleByDoctorScheduleId(int doctorScheduleId)
        {
            // Check if schedule exists
            var doctorSchedule = await _doctorScheduleRepository.GetDoctorScheduleByIdAsync(doctorScheduleId);
            if (doctorSchedule == null)
            {
                return NotFound(_messageService.GetMessage("DoctorScheduleNotFound"));
            }

            return Ok(doctorSchedule);
        }


        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllSchedules([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Check if doctor has a schedule
            var doctorSchedules = await _doctorScheduleRepository.GetAllSchedulesAsync();

            if (doctorSchedules == null || doctorSchedules.Count == 0)
            {
                return NotFound(_messageService.GetMessage("DoctorScheduleNotFound"));
            }
            try
            {
                var doctorSchedule = await _doctorScheduleManager.GetDoctorScheduleAsync(doctorSchedules, page, pageSize);
                return Ok(doctorSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
