using System;
using System.Threading.Tasks;
using CMD.Domain.DTO;

namespace CMD.Domain.Services
{
    /// <summary>
    /// Defines the contract for a repository that manages clinic data.
    /// </summary>
    public interface IClinicRepository
    {
        /// <summary>
        /// Asynchronously checks if a clinic with the specified ID is valid.
        /// </summary>
        /// <param name="clinicId">The unique identifier of the clinic to validate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <c>true</c> if the clinic is valid (i.e., exists and is accessible); otherwise, <c>false</c>.
        /// </returns>
        Task<bool> IsValidClinicAsync(int clinicId);

        /// <summary>
        /// Asynchronously retrieves the address of a clinic with the specified ID.
        /// </summary>
        /// <param name="clinicId">The unique identifier of the clinic whose address is to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a <see cref="ClinicAddressDto"/> object if the clinic is found; otherwise, <c>null</c>.
        /// </returns>
        /// <remarks>
        /// The address is extracted from the clinic details retrieved from the remote API.
        /// </remarks>
        Task<ClinicAddressDto> GetClinicAddressAsync(int clinicId);
    }
}
