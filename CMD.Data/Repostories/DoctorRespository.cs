﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Data.Context;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;

namespace CMD.Data.Repostories
{
    public class DoctorRespository : IDoctorRepository
    {
        private readonly DoctorDbContext _context;

        public DoctorRespository(DoctorDbContext context)
        {
            this._context = context;
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }
    }
}
