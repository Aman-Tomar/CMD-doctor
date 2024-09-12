using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.DTO;
using Newtonsoft.Json;

namespace CMD.Domain.Services
{
    public class ClinicRepository : IClinicRepository
    {
        private readonly HttpClient _httpClient;

        public ClinicRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsValidClinic(int clinicId)
        {
            var response = await _httpClient.GetAsync($"/api/Clinic/{clinicId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<ClinicAddressDto> GetClinicAddress(int clinicId)
        {
            var response = await _httpClient.GetAsync($"/api/Clinic/{clinicId}");
            if (response.IsSuccessStatusCode)
            {
                var clinic = JsonConvert.DeserializeObject<ClinicDto>(await response.Content.ReadAsStringAsync());
                return clinic?.ClinicAddress;
            }
            return null;
        }
    }
}
