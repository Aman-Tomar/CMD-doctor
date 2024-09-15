using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.Enums;

namespace CMD.Domain.Entities
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        [Required(ErrorMessage = "FirstName field is mandatory.")]
        [StringLength(50, ErrorMessage = "Number of characters must be less than 50.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName field is mandatory.")]
        [StringLength(50, ErrorMessage = "LastName must be less than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email field is mandatory.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PhoneNo field is mandatory.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Specialization field is mandatory.")]
        [StringLength(100, ErrorMessage = "Specialization must be less than 100 characters.")]
        public string Specialization { get; set; }
        public byte[]? ProfilePicture { get; set; }

        [StringLength(500, ErrorMessage = "Brief description must be less than 500 characters.")]
        public string BriefDescription { get; set; }

        [Range(0, 100, ErrorMessage = "Experience must be between 0 and 100 years.")]
        public int ExperienceInYears { get; set; }

        [Required(ErrorMessage = "DOB field is mandatory.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender field is mandatory.")]
        [Column(TypeName = "nvarchar(50)")]
        public Gender Gender { get; set; }

        [StringLength(100, ErrorMessage = "Qualification must be less than 100 characters.")]
        public string Qualification { get; set; }
        public bool Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "CreatedBy must be less than 50 characters.")]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime LastModifiedDate { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "LastModifiedBy must be less than 50 characters.")]
        public string LastModifiedBy { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int ClinicId { get; set; }
        public DoctorAddress DoctorAddress { get; set; }
    }
}
