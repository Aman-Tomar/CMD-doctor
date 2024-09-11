using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.Entities;

namespace CMD.Domain.Services
{
    public interface IManageDoctorSchedule
    {
        Task<bool> IsAvailable(DoctorSchedule doctorSchedule);
    }
}
