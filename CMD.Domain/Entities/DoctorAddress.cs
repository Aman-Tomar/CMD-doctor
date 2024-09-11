using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Domain.Entities
{
    public class DoctorAddress
    {
        [Key]
        public int DoctorAddressId { get; set; }
        [Required(ErrorMessage ="Street is a required field")]
        public string Street { get; set; }
        [Required(ErrorMessage = "City is a required field")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is a required field")]
        public string State { get; set; }
        [Required(ErrorMessage = "Country is a required field")]
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
