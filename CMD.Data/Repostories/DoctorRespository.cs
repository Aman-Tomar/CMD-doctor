using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Data.Context;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CMD.Data.Repostories
{
    public class DoctorRespository : IDoctorRepository
    {
        private readonly DoctorDbContext db;

        public DoctorRespository(DoctorDbContext db)
        {
            this.db = db;
        }
        public async Task<Doctor> GetDoctorById(int id)
        {
            return await db.Doctors
                .Include(d=>d.DoctorAddress)
                .FirstOrDefaultAsync(d=>d.DoctorId == id);
        }
    }
}
