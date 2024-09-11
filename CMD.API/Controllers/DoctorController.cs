using CMD.API.DTO;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
        // POST ../api/Doctor/Add - FE001
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddDoctor()
        {
            return Ok();
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
        public async Task<IActionResult> GetAllDoctor()
        {
            return Ok();

        }
        // GET ../api/Doctor/id - FE003
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            return Ok();
        }
    }
}
