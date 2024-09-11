using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Data.Context;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;

namespace CMD.Data.Repostories
{
    public class DoctorScheduleRepository : IDoctorScheduleRepository
    {
        private readonly DoctorDbContext _context;

        public DoctorScheduleRepository(DoctorDbContext context)
        {
            this._context = context;
        }

        public async Task<DoctorSchedule> GetDoctorScheduleById(int doctorScheduleId)
        {
            return await _context.DoctorSchedules.FindAsync(doctorScheduleId);
        }

        public async Task UpdateDoctorSchedule(DoctorSchedule doctorSchedule)
        {
            _context.DoctorSchedules.Update(doctorSchedule);
            await _context.SaveChangesAsync();
        }
    }
}
