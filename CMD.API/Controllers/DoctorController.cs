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
        public async Task<IActionResult> AddDoctor(AddDoctorDto doctor)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[]? imageBytes = null;

            if (doctor.ProfilePicture != null && doctor.ProfilePicture.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await doctor.ProfilePicture.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }
            }

            Doctor doc = new Doctor
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                BriefDescription = doctor.Biography,
                DateOfBirth = doctor.DOB,
                Email = doctor.Email,
                Gender = doctor.Gender,
                PhoneNo = doctor.Phone,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity?.Name,
                LastModifiedBy = User.Identity?.Name,
                ProfilePicture = imageBytes,
                Status = doctor.Status,
                DoctorAddress  = new DoctorAddress
                {
                    State = doctor.Address,
                    City = doctor.City,
                    LastModifiedDate = DateTime.Now,
                    Country = doctor.Country,
                    ZipCode = doctor.ZipCode
                }
            };
            repo.AddDoctorAsync(doc);
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
