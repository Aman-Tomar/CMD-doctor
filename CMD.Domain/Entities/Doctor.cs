using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Domain.Entities
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        [Required(ErrorMessage ="FirstName is a required field")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Specialization { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string BriefDescription { get; set; }
        public int ExperienceInYears { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string? Qualification { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DoctorAddress DoctorAddress { get; set; }
    }
}
