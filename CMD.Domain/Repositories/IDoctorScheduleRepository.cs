using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.Entities;

namespace CMD.Domain.Repositories
{
    public interface IDoctorScheduleRepository
    {
        Task UpdateDoctorSchedule(DoctorSchedule doctorSchedule);
        Task<DoctorSchedule> GetDoctorScheduleById(int doctorScheduleId);
        Task<List<DoctorSchedule>> GetDoctorScheduleForWeekday(int doctorId, string weekday);
    }
}
