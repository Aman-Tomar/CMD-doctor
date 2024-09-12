using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Domain.Entities
{
    public class DoctorSchedule
    {
        [Key]
        [Required]
        public int DoctorScheduleId { get; set; }
        [StringLength(300, ErrorMessage = "The length must be less than 300 characters.")]
        public string? Clinic { get; set; }
        [Required(ErrorMessage = "Weeday field is mandatory.")]
        public string Weekday { get; set; }
        [Required(ErrorMessage = "StartTime field is mandatory.")]
        public TimeOnly StartTime { get; set; }
        [Required(ErrorMessage = "EndTime field is mandatory.")]
        public TimeOnly EndTime { get; set; }
        [Required(ErrorMessage = "Status field is mandatory.")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "CreatedDate field is mandatory.")]
        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "CreatedBy field is mandatory.")]
        [StringLength(50, ErrorMessage = "The length must be less than 50 characters.")]
        public string CreatedBy { get; set; }
        [Required(ErrorMessage = "LastModifiedDate field is mandatory.")]
        public DateTime LastModifiedDate { get; set; }
        [Required(ErrorMessage = "LastModifiedBy field is mandatory.")]
        public string LastModifiedBy { get; set; }
        [Required(ErrorMessage = "Doctor field is mandatory.")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
