using CMD.Domain.DTO;
using CMD.Domain.Entities;
using CMD.Domain.Managers;
using CMD.Domain.Repositories;
using CMD.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;

namespace CMD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DoctorScheduleController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
            _doctorScheduleRepository = doctorScheduleRepository;
            _doctorRepository = doctorRepository;
            _doctorScheduleManager = doctorScheduleManager;
            _messageService = messageService;
        }

        /// <summary>
        /// Creates a new doctor schedule.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor for whom the schedule is being created.</param>
        /// <param name="doctorScheduleDto">The <see cref="DoctorScheduleDto"/> containing the details of the schedule to be created.</param>
        /// <returns>A 201 Created status code if the schedule is successfully created; a 400 Bad Request status code if the input data is invalid or an exception occurs during processing.</returns>
        /// <response code="201">Returned when the doctor schedule is successfully created.</response>
        /// <response code="400">Returned when the input data is invalid or when an error occurs during processing.</response>
        // [Authorize(Roles = "Doctor, Admin")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDoctorSchedule([FromQuery] int doctorId, [FromBody] DoctorScheduleDto doctorScheduleDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warn("Model state is invalid");
                return BadRequest(ModelState);
            }

            var doctor = await _doctorRepository.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                _logger.Warn($"Doctor not found. ID: {doctorId}");
                return NotFound(_messageService.GetMessage("DoctorNotFound"));
            }

            try
            {
                var doctorSchedule = await _doctorScheduleManager.CreateDoctorScheduleAsync(doctorId, doctorScheduleDto);
                var locationUri = $"api/DoctorSchedule/{doctorSchedule.DoctorScheduleId}";
                _logger.Info($"Doctor added successfully. ID: {doctor.DoctorId}");
                return Created(locationUri, doctorSchedule);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while adding doctor");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing doctor schedule.
        /// </summary>
        /// <param name="doctorScheduleId">The unique identifier of the doctor schedule to be updated.</param>
        /// <param name="doctorScheduleDto">The <see cref="DoctorScheduleDto"/> containing the updated details for the doctor schedule.</param>
        /// <returns>A 200 OK status code if the schedule is successfully updated; a 400 Bad Request status code if the input data is invalid or an exception occurs during processing; a 404 Not Found status code if the schedule or doctor with the provided ID does not exist.</returns>
        /// <response code="200">Returned when the doctor schedule is successfully updated.</response>
        /// <response code="400">Returned when the input data is invalid or when an error occurs during processing.</response>
        /// <response code="404">Returned when the doctor schedule with the specified ID or the doctor with the specified ID is not found.</response>
        // [Authorize(Roles = "Doctor, Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditDoctorSchedule([FromQuery] int doctorScheduleId, [FromBody] DoctorScheduleDto doctorScheduleDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warn("Model state is invalid");
                return BadRequest(ModelState);
            }

            var doctor = await _doctorRepository.GetDoctorByIdAsync(doctorScheduleDto.DoctorId);
            if (doctor == null)
            {
                _logger.Warn($"Doctor not found. ID: {doctorScheduleDto.DoctorId}");
                return NotFound(_messageService.GetMessage("DoctorNotFound"));
            }

            var existingDoctorSchedule = await _doctorScheduleRepository.GetDoctorScheduleByIdAsync(doctorScheduleId);
            if (existingDoctorSchedule == null)
            {
                _logger.Warn($"Doctor Schedule not found. ID: {doctorScheduleId}");
                return NotFound(_messageService.GetMessage("DoctorScheduleNotFound"));
            }

            try
            {
                var doctorSchedule = await _doctorScheduleManager.EditDoctorScheduleAsync(doctorScheduleDto, existingDoctorSchedule, doctor);
                _logger.Info($"Doctor schedule updated successfully. ID: {doctor.DoctorId}");
                return Ok(doctorSchedule);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while adding doctor");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the schedule for a specific doctor.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose schedule is being retrieved.</param>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <returns>A 200 OK status code with the doctor's schedule if found; a 404 Not Found status code if the doctor does not exist or if no schedule is available for the doctor.</returns>
        /// <response code="200">Returned when the doctor's schedule is successfully retrieved.</response>
        /// <response code="404">Returned when the doctor with the specified ID is not found or if no schedule is available for the doctor.</response>
        // [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDoctorSchedule([FromQuery] int doctorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                _logger.Warn($"Doctor not found. ID: {doctorId}");
                return NotFound(_messageService.GetMessage("DoctorNotFound"));
            }

            var doctorSchedules = await _doctorScheduleRepository.GetScheduleByDoctorIdAsync(doctorId);

            if (doctorSchedules == null || doctorSchedules.Count == 0)
            {
                _logger.Warn($"Doctor Schedules not found.");
                return NotFound(_messageService.GetMessage("DoctorScheduleNotFound"));
            }

            try
            {
                var doctorSchedule = await _doctorScheduleManager.GetDoctorScheduleAsync(doctorSchedules, page, pageSize);
                _logger.Info("Retrieved paginated list of doctor schedules");
                return Ok(doctorSchedule);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while adding doctor");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific doctor schedule by ID.
        /// </summary>
        /// <param name="doctorScheduleId">The unique identifier of the doctor schedule to retrieve.</param>
        /// <returns>A 200 OK status code with the doctor schedule if found; a 404 Not Found status code if the doctor schedule does not exist.</returns>
        /// <response code="200">Returned when the doctor schedule is successfully retrieved.</response>
        /// <response code="404">Returned when the doctor schedule with the specified ID is not found.</response>
        // [Authorize]
        [HttpGet("{doctorScheduleId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetScheduleByDoctorScheduleId(int doctorScheduleId)
        {
            try
            {
                var doctorSchedule = await _doctorScheduleRepository.GetDoctorScheduleByIdAsync(doctorScheduleId);
                if (doctorSchedule == null)
                {
                    _logger.Warn($"Doctor Schedule not found. ID: {doctorScheduleId}");
                    return NotFound(_messageService.GetMessage("DoctorScheduleNotFound"));
                }
                _logger.Info("Retrieved doctor schedule");
                return Ok(doctorSchedule);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while adding doctor");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all doctor schedules with pagination.
        /// </summary>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <returns>A 200 OK status code with all doctor schedules if found; a 404 Not Found status code if no schedules are available.</returns>
        /// <response code="200">Returned when the doctor schedules are successfully retrieved.</response>
        /// <response code="404">Returned when no doctor schedules are found.</response>
        // [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllSchedules([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var doctorSchedules = await _doctorScheduleRepository.GetAllSchedulesAsync();

            if (doctorSchedules == null || doctorSchedules.Count == 0)
            {
                    _logger.Warn($"Doctor Schedules not found.");
                return NotFound(_messageService.GetMessage("DoctorScheduleNotFound"));
            }

            try
            {
                var doctorSchedule = await _doctorScheduleManager.GetDoctorScheduleAsync(doctorSchedules, page, pageSize);
                _logger.Info("Retrieved doctorSchedule");
                return Ok(doctorSchedule);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while adding doctor");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Checks if a doctor has a schedule on a specific date.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor.</param>
        /// <param name="date">The date to check the schedule for.</param>
        /// <returns>A 200 OK status code with a boolean indicating if the doctor has a schedule for that date; a 404 Not Found status code if the doctor does not exist.</returns>
        /// <response code="200">Returned when the schedule is found for the specified date.</response>
        /// <response code="404">Returned when the doctor with the specified ID is not found.</response>
        // [Authorize]
        [HttpGet("available")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CheckDoctorSchedule([FromQuery] int doctorId, [FromQuery] DateOnly date, [FromQuery] TimeOnly startTime, [FromQuery] TimeOnly endTime)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                _logger.Warn($"Doctor not found. ID: {doctorId}");
                return NotFound(_messageService.GetMessage("DoctorNotFound"));
            }

            var scheduleExists = await _doctorScheduleRepository.DoesScheduleExistAsync(doctorId, date, startTime, endTime);
            return Ok(scheduleExists);
        }
    }
}
