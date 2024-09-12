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
        public async Task<List<DoctorDto>> GetAllDoctorsAsync(int pageNumber, int pageSize)
        {
            return await _context.Doctors
                        .Include(d => d.DoctorAddress)   // Ensure DoctorAddress is loaded
                        .Skip((pageNumber - 1) * pageSize)  // Skip previous pages' records
                        .Take(pageSize)                     // Take only the required number of records
                        .Select(d => new DoctorDto          // Project the result into DoctorDto
                        {
                            //ProfilePicture = d.ProfilePicture,
                            FirstName = d.FirstName,
                            LastName = d.LastName,
                            Specialization = d.Specialization,
                            City = d.DoctorAddress.City     // Access the City from DoctorAddress
                        })
                        .ToListAsync();
        }
        public async Task<int> GetTotalNumberOfDoctorsAsync()
        {
            return await _context.Doctors.CountAsync();
        }
        public async Task EditDoctor(Doctor doctor)
        {
            db.Doctors.Update(doctor);
            await db.SaveChangesAsync();
        }

        public async Task<Doctor> GetDoctorById(int id)
        {
            return await db.Doctors
                .Include(d=>d.DoctorAddress)
                .FirstOrDefaultAsync(d=>d.DoctorId==id);

        public async Task AddDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }
    }
}
