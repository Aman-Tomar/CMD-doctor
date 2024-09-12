using CMD.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CMD.Domain.DTO
{
    public class DoctorScheduleDto
    {
        [Required(ErrorMessage = "ClinicId field is mandatory.")]
        public int ClinicId { get; set; }
        [Required(ErrorMessage = "Weeday field is mandatory.")]
        public string Weekday { get; set; }
        [Required(ErrorMessage = "StartTime field is mandatory.")]
        public string StartTime { get; set; }
        [Required(ErrorMessage = "EndTime field is mandatory.")]
        public string EndTime { get; set; }
        [Required(ErrorMessage = "Status field is mandatory.")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "DoctorId field is mandatory.")]
        public int DoctorId { get; set; }
    }
}
