using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CMD.Domain.DTO
{
    public class DoctorDto
    {
        [Required(ErrorMessage = "FirstName field is mandatory.")]
        [StringLength(50, ErrorMessage = "Number of characters must be less than 50.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName field is mandatory.")]
        [StringLength(50, ErrorMessage = "Number of characters must be less than 50.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "DOB field is mandatory.")]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Email field is mandatory.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Gender field is mandatory.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Address field is mandatory.")]
        [StringLength(100, ErrorMessage = "Number of characters must be less than 100.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Country field is mandatory.")]
        public string Country { get; set; }
        [Required(ErrorMessage = "City field is mandatory.")]
        public string City { get; set; }
        [Required(ErrorMessage = "ZipCode field is mandatory.")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "State field is mandatory.")]
        public string State { get; set; }
        [Required(ErrorMessage = "Biography field is mandatory.")]
        [StringLength(200, ErrorMessage = "Number of characters must be less than 200.")]
        public string Biography { get; set; }
        [Required(ErrorMessage = "Phone field is mandatory.")]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage = "Status field is mandatory.")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "Specialization field is mandatory.")]
        [StringLength(100, ErrorMessage = "Number of characters must be less than 100.")]
        public string Specialization { get; set; }
        [Required(ErrorMessage = "ExperienceInYears field is mandatory.")]
        public int ExperienceInYears { get; set; }
        public IFormFile? profilePicture { get; set; }
        public string? Qualification { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int ClinicId { get; set; }
    }
}
