using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.DTO;

namespace CMD.Domain.Services
{
    /// <summary>
    /// Repository class for managing department data by interacting with a remote API.
    /// Implements the <see cref="IDepartmentRepository"/> interface.
    /// </summary>
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make HTTP requests to the remote API.</param>
        public DepartmentRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Checks if a department with the specified ID is valid.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to validate.</param>
        /// <returns>
        /// <c>true</c> if the department is valid (i.e., exists and is accessible); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Sends a GET request to the remote API to check if the department exists.
        /// </remarks>
        public async Task<bool> IsValidDepartmentAsync(int departmentId)
        {
            var response = await _httpClient.GetAsync($"https://cmd-clinic-api.azurewebsites.net/api/Department/{departmentId}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
