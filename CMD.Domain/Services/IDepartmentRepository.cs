using System;
using System.Threading.Tasks;
using CMD.Domain.DTO;

namespace CMD.Domain.Services
{
    /// <summary>
    /// Defines the contract for a repository that manages department data.
    /// </summary>
    public interface IDepartmentRepository
    {
        /// <summary>
        /// Asynchronously checks if a department with the specified ID is valid.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to validate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <c>true</c> if the department is valid (i.e., exists and is accessible); otherwise, <c>false</c>.
        /// </returns>
        Task<bool> IsValidDepartmentAsync(int departmentId);
    }
}
