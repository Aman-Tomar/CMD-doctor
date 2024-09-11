﻿using CMD.API.Models.DTO;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetAllDoctor()
        {
            return Ok();

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
