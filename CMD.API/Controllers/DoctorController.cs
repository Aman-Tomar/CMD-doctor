using CMD.Domain.DTO;
using CMD.Domain.Entities;
using CMD.Domain.Managers;
using CMD.Domain.Repositories;
using CMD.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CMD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DoctorController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorManager _doctorManager;
        private readonly IMessageService _messageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorController"/> class.
        /// </summary>
        /// <param name="doctorRepository">The repository for interacting with doctor data.</param>
        /// <param name="doctorManager">The manager for handling doctor-related business logic.</param>
        /// <param name="messageService">The service for providing custom error messages.</param>
        public DoctorController(IDoctorRepository doctorRepository, IDoctorManager doctorManager, IMessageService messageService)
        {
            this._doctorRepository = doctorRepository;
            this._doctorManager = doctorManager;
            this._messageService = messageService;
        }

        /// <summary>
        /// Adds a new doctor to the system.
        /// </summary>
        /// <param name="doctorDto">The <see cref="DoctorDto"/> containing the details of the doctor to be added.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        /// <response code="201">Doctor successfully created.</response>
        /// <response code="400">Bad request if the model state is invalid or an error occurs.</response>
        // [Authorize(Roles = "Doctor, Admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDoctor([FromForm] DoctorDto doctorDto)
        {
            // Check if all the properties are provided
            if (!ModelState.IsValid)
            {
                _logger.Warn("Model state is invalid");
                return BadRequest(ModelState);
            }

            try
            {
                var doctor = await _doctorManager.AddDoctorAsync(doctorDto);
                var locationUri = $"api/Doctor/{doctor.DoctorId}";
                _logger.Info($"Doctor added successfully. ID: {doctor.DoctorId}");
                return Created(locationUri, doctor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while adding doctor");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing doctor in the system.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor to be updated.</param>
        /// <param name="doctorDto">The <see cref="DoctorDto"/> containing updated details for the doctor.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        /// <response code="200">Doctor successfully updated.</response>
        /// <response code="404">Doctor not found.</response>
        /// <response code="400">Bad request if the model state is invalid or an error occurs.</response>
        // [Authorize(Roles = "Doctor, Admin")]
        [HttpPut]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditDoctor([FromQuery] int doctorId, [FromForm] DoctorDto doctorDto)
        {
            // Check if all the properties are provided
            if (!ModelState.IsValid)
            {
                _logger.Warn("Model state is invalid");
                return BadRequest();
            }

            // Check if doctor exists
            var existingDoctor = await _doctorRepository.GetDoctorByIdAsync(doctorId);
            if (existingDoctor == null)
            {
                _logger.Warn($"Doctor not found. ID: {doctorId}");
                return NotFound(_messageService.GetMessage("DoctorNotFound"));
            }

            try
            {
                var doctor = await _doctorManager.EditDoctorAsync(existingDoctor, doctorDto);
                _logger.Info($"Doctor updated successfully. ID: {doctor.DoctorId}");
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating doctor");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of all doctors.
        /// </summary>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of doctors.</returns>
        /// <response code="200">Successfully retrieved the list of doctors.</response>
        /// <response code="404">No doctors found.</response>
        /// <response code="400">Bad request if an error occurs.</response>
        // [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDoctors([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var doctors = await _doctorRepository.GetAllDoctorsAsync();
            if (doctors == null || doctors.Count == 0)
            {
                _logger.Warn("No doctors found");
                return NotFound(_messageService.GetMessage("DoctorNotFound"));
            }
            try
            {
                var doctorSchedule = await _doctorManager.GetAllDoctorAsync(doctors, page, pageSize);
                _logger.Info("Retrieved paginated list of doctors");
                return Ok(doctorSchedule);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while retrieving doctors");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific doctor by ID.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the details of the doctor.</returns>
        /// <response code="200">Successfully retrieved the doctor.</response>
        /// <response code="404">Doctor not found.</response>
        // [Authorize]
        [HttpGet("{doctorId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDoctorById(int doctorId)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                _logger.Warn($"Doctor not found. ID: {doctorId}");
                return NotFound(_messageService.GetMessage("DoctorNotFound"));
            }

            _logger.Info($"Doctor retrieved successfully. ID: {doctorId}");
            return Ok(doctor);
        }
    }
}
