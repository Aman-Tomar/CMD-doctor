using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorScheduleController : ControllerBase
    {
        // POST ../api/DoctorSchedule/Add - FE007
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddDoctorSchedule()
        {
            return Ok();
        }

        // PUT ../api/DoctorSchedule/Edit - FE008
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditDoctorSchedule()
        {
            return Ok();
        }

        // GET ../api/DoctorSchedule - FE009
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDoctorSchedule()
        {
            return Ok();
        }
    }
}
