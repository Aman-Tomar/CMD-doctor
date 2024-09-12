using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.DTO;

namespace CMD.Domain.Services
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HttpClient _httpClient;

        public DepartmentRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsValidDepartment(int departmentId)
        {
            var response = await _httpClient.GetAsync($"api/Department/{departmentId}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
