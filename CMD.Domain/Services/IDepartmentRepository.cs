using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.DTO;

namespace CMD.Domain.Services
{
    public interface IDepartmentRepository
    {
        Task<bool> IsValidDepartment(int departmentId);
    }
}
