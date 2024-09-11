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

        // PUT ../api/DoctorSchedule/Edit - FE008
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditDoctorSchedule()
        {
            return Ok();
        }

        /// <summary>
        /// Retrieves the schedule of a specific doctor.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches the schedule associated with a doctor based on the provided doctor ID. If the doctor exists and has a schedule, it returns the schedule details. If the doctor does not exist or if no schedule is available for the doctor, an appropriate error response is returned.
        /// </remarks>
        /// <param name="doctorId">The unique identifier of the doctor whose schedule is being retrieved.</param>
        /// <returns>
        /// A 200 OK status code with the doctor's schedule if found.
        /// A 404 Not Found status code if the doctor does not exist or if the doctor has no schedule.
        /// </returns>
        /// <response code="200">Returned when the doctor's schedule is successfully retrieved.</response>
        /// <response code="404">Returned when the doctor with the specified ID is not found or if no schedule is available for the doctor.</response>
        /// GET ../api/DoctorSchedule?id=123
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDoctorSchedule([FromQuery] int doctorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Check if doctor exists
            var doctor = _doctorRepository.GetDoctorById(doctorId);
            if (doctor == null)
            {
                return NotFound("Doctor Not Found");
            }

            // Check if doctor has a schedule
            var doctorScheduleQuery = await _doctorScheduleRepository.GetScheduleByDoctorId(doctorId);
            if (doctorScheduleQuery == null)
            {
                return NotFound("The doctor has no schedule");
            }

            // Apply pagination
            var doctorSchedule = doctorScheduleQuery.Skip((page - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .ToList();

            var result = new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = doctorScheduleQuery.Count(),
                TotalPages = (int)Math.Ceiling((double)doctorScheduleQuery.Count() / pageSize),
                Data = doctorSchedule
            };

            return Ok(result);
        }
    }
}
