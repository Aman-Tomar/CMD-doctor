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
        Task CreateDoctorSchedule(DoctorSchedule doctorSchedule);
    }
}
