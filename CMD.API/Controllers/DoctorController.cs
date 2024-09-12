
using CMD.Data.Repostories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CMD.Domain.DTO;
using CMD.Domain.Repositories;

namespace CMD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _repo;
        public DoctorController(IDoctorRepository repo)
        {
            _repo = repo;
        }
        // POST ../api/Doctor/Add - FE001
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddDoctor()
        {
            return Ok();
        }

        // PUT ../api/Doctor/Edit - FE002, FE005, FE006
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditDoctor()
        {
            return Ok();
        }

        // GET ../api/Doctor - FE004
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDoctors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1)
        {
            // Validating page size and page number
            var allowedPageSizes = new[] {1,2, 20, 40, 60, 100 };
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;

            // Default to 20 if the pageSize is not allowed
            if (!allowedPageSizes.Contains(pageSize))
            {
                pageSize = 1;
            }

            // Fetch paginated doctors and total number of doctors
            var doctors = await _repo.GetAllDoctorsAsync(pageNumber, pageSize);
            Console.WriteLine($"Doctors retrieved: {doctors?.Count ?? 0}");
            var totalDoctors = await _repo.GetTotalNumberOfDoctorsAsync();
            Console.WriteLine($"Total doctors: {totalDoctors}");

            // Return 404 if no doctors were found
            if (doctors == null || !doctors.Any())
            {
                return NotFound(new { Message = "No doctors found" });
            }

            // Construct the response object with pagination metadata
            var result = new
            {
                TotalDoctors = totalDoctors,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Doctors = doctors // The doctors is mapped to DoctorDto in the data layer
            };

            return Ok(result); // Return the paginated result with 200 OK


        }
        // GET ../api/Doctor/id - FE003
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDoctorById()
        {
            return Ok();
        }
    }
    
}
