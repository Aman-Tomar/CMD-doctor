using CMD.API.DTO;
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

        public DoctorScheduleController(IDoctorScheduleRepository doctorScheduleRepository)
        {
            this._doctorScheduleRepository = doctorScheduleRepository;
        }

        /// <summary>
        /// Creates a new doctor schedule in the system.
        /// </summary>
        /// <remarks>
        /// This endpoint allows the creation of a doctor schedule by providing the necessary details such as available time slots, dates, clinic locations, and services offered. The request must include a valid JSON payload representing the doctor's schedule. If the schedule is created successfully, the response will be a 201 Created status. If the request contains invalid data or an error occurs during processing, a corresponding error response will be returned.
        /// </remarks>
        /// <param name="doctorSchedule">The schedule information to be created, including details like available time slots, dates, clinic locations, and services offered.</param>
        /// <returns>
        /// A 201 Created status code if the schedule is successfully created.
        /// A 400 Bad Request status code if the input data is invalid or if an exception occurs during the operation.
        /// </returns>
        /// <response code="201">Returned when the doctor schedule is successfully created.</response>
        /// <response code="400">Returned when the input data is invalid or when an error occurs during processing.</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDoctorSchedule(DoctorSchedule doctorSchedule)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _doctorScheduleRepository.CreateDoctorSchedule(doctorSchedule);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT ../api/DoctorSchedule/Edit - FE008
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditDoctorSchedule()
        {
            return Ok();
        }

        // GET ../api/DoctorSchedule - FE009
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDoctorSchedule()
        {
            return Ok();
        }
    }
}
