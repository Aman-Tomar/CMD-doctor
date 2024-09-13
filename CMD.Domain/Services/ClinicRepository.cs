using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.DTO;
using Newtonsoft.Json;

namespace CMD.Domain.Services
{
    /// <summary>
    /// Repository class for managing clinic data by interacting with a remote API.
    /// Implements the <see cref="IClinicRepository"/> interface.
    /// </summary>
    public class ClinicRepository : IClinicRepository
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClinicRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make HTTP requests to the remote API.</param>
        public ClinicRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Asynchronously checks if a clinic with the specified ID is valid.
        /// </summary>
        /// <param name="clinicId">The unique identifier of the clinic to validate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <c>true</c> if the clinic is valid (i.e., exists and is accessible); otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsValidClinicAsync(int clinicId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44365/api/Clinic/{clinicId}");
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Asynchronously retrieves the address of a clinic with the specified ID.
        /// </summary>
        /// <param name="clinicId">The unique identifier of the clinic whose address is to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a <see cref="ClinicAddressDto"/> object if the clinic is found; otherwise, <c>null</c>.
        /// </returns>
        /// <remarks>
        /// Sends a GET request to the remote API to retrieve the clinic's details and extracts the clinic address from the response.
        /// </remarks>
        public async Task<ClinicAddressDto> GetClinicAddressAsync(int clinicId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44365/api/Clinic/{clinicId}");
            if (response.IsSuccessStatusCode)
            {
                var clinic = JsonConvert.DeserializeObject<ClinicDto>(await response.Content.ReadAsStringAsync());
                return clinic?.ClinicAddress;
            }
            return null;
        }
    }
}
