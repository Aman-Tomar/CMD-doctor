using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Data.Context;
using CMD.Domain.Entities;
using CMD.Data.Context;
using CMD.Domain.Entities;
using CMD.Data.Context;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using CMD.Domain.DTO;

namespace CMD.Data.Repostories
{
    public class DoctorRespository : IDoctorRepository
    {
        private readonly DoctorDbContext _context;
        public DoctorRespository(DoctorDbContext context)
        {
            this._context = context;
        }
        public async Task<Doctor> GetDoctorById(int doctorId)
        {
            return await _context.Doctors.FindAsync(doctorId);
        }
        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
                        .Include(d => d.DoctorAddress) 
                        .ToListAsync();
        }
        public async Task EditDoctor(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }
    }
}
