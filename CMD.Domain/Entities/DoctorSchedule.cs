using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Domain.Entities
{
    public class DoctorSchedule
    {
        public int DoctorScheduleId { get; set; }
        public string? Clinic { get; set; }
        public string Weekday { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
