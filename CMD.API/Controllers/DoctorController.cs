﻿using CMD.Domain.Entities;
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
        public async Task<IActionResult> GetAllDoctor()
        {
            return Ok();

        }
        // GET ../api/Doctor/id - FE003
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
