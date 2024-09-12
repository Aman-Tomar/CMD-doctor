using CMD.API.DTO;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using CMD.Domain.DTO;
using CMD.Domain.Repositories;

namespace CMD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository repo;
        public DoctorController(IDoctorRepository repo)
        {
            this.repo = repo;
        }
        // POST ../api/Doctor/Add
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDoctor([FromBody]AddDoctorDto doctor)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Doctor doc = new Doctor
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                BriefDescription = doctor.Biography,
                DateOfBirth = doctor.DOB,
                Email = doctor.Email,
                Gender = doctor.Gender,
                Qualification = doctor.Qualification,
                ExperienceInYears = doctor.ExperienceInYears,
                Specialization = doctor.Specialization,
                PhoneNo = doctor.Phone,
                CreatedAt = DateTime.Now,
                CreatedBy = "admin",//User.Identity?.Name,
                LastModifiedBy = "admin",//User.Identity?.Name,
                //ProfilePicture = imageBytes,
                Status = doctor.Status,
                DoctorAddress = new DoctorAddress
                {
                    Street = doctor.Address,
                    City = doctor.City,
                    LastModifiedDate = DateTime.Now,
                    Country = doctor.Country,
                    ZipCode = doctor.ZipCode,
                    CreatedBy = "admin",
                    LastModifiedBy = "admin",
                    CreatedDate = DateTime.Now,
                    State = doctor.State

                }
            };
            await repo.AddDoctorAsync(doc);
            return Created($"api/Doctor/add/{doc.DoctorId}",doc);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditDoctor([FromQuery]int id,[FromBody] EditDto doctor)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var doc = await repo.GetDoctorById(id);
            if (doc == null)
            {
                return NotFound();
            }
           
            // mapping the DTO
            doc.FirstName = doctor.FirstName;
            doc.LastName = doctor.LastName;
            doc.BriefDescription = doctor.Biography;
            doc.DateOfBirth = doctor.DOB;
            doc.Email = doctor.Email;
            doc.Gender = doctor.Gender;
            doc.PhoneNo = doctor.Phone;
            doc.Status = doctor.IsActive;
            doc.Specialization = doctor.Specialization;
            doc.Qualification = doctor.Qualification;
            doc.ExperienceInYears = doctor.ExperienceInYears;
            doc.LastModifiedBy = "admin";// User.Identity?.Name;
          //  doc.ProfilePicture = doctor.ProfilePicture; 
            doc.DoctorAddress.Street = doctor.Address;
            doc.DoctorAddress.City = doctor.City;
            doc.DoctorAddress.State = doctor.State;
            doc.DoctorAddress.Country = doctor.Country;
            doc.DoctorAddress.ZipCode = doctor.ZipCode;
            doc.DoctorAddress.LastModifiedBy = "admin";//User.Identity?.Name;
            doc.DoctorAddress.LastModifiedDate = DateTime.Now; 
            await repo.EditDoctor(doc);
            return Ok(doc);
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
        public async Task<IActionResult> GetDoctorById(int id)
        {
            Doctor doctor = await repo.GetDoctorById(id);
            if(doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }
    }
    
}
