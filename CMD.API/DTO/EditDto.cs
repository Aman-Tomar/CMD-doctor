namespace CMD.API.DTO
{
    public class EditDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Biography { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
        public string Specialization { get; set; }
        public string? Qualification { get; set; }
        public int ExperienceInYears { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
