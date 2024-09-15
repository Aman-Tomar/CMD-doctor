using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMD.Domain.DTO;
using CMD.Domain.Entities;
using CMD.Domain.Enums;
using CMD.Domain.Exceptions;
using CMD.Domain.Repositories;
using CMD.Domain.Services;
using CMD.Domain.Validator;
using Microsoft.AspNetCore.Http;

namespace CMD.Domain.Managers
{
    /// <summary>
    /// Manages operations related to doctors, including adding, editing, and retrieving doctors.
    /// </summary>
    public class DoctorManager : IDoctorManager
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IClinicRepository _clinicRepository;
        private readonly IMessageService _messageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorManager"/> class.
        /// </summary>
        /// <param name="doctorRepository">The repository for interacting with doctor data.</param>
        /// <param name="departmentRepository">The repository for interacting with department data.</param>
        /// <param name="clinicRepository">The repository for interacting with clinic data.</param>
        /// <param name="messageService">The service for providing custom error messages.</param>
        public DoctorManager(IDoctorRepository doctorRepository, IDepartmentRepository departmentRepository, IClinicRepository clinicRepository, IMessageService messageService)
        {
            this._doctorRepository = doctorRepository;
            this._departmentRepository = departmentRepository;
            this._clinicRepository = clinicRepository;
            this._messageService = messageService;
        }

        /// <summary>
        /// Adds a new doctor to the system.
        /// </summary>
        /// <remarks>
        /// Validates the provided doctor details including name, date of birth, email, and phone number. Maps the data from the <see cref="DoctorDto"/> to a new <see cref="Doctor"/> entity and adds it to the repository.
        /// </remarks>
        /// <param name="doctorDto">The <see cref="DoctorDto"/> containing the details of the doctor to be added.</param>
        /// <returns>
        /// A <see cref="Doctor"/> object representing the newly added doctor.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the input data is invalid, such as incorrect name format, invalid date of birth, email, or phone number.</exception>
        public async Task<Doctor> AddDoctorAsync(DoctorDto doctorDto)
        {
            // Validate Name
            if (!DoctorValidator.IsValidName(doctorDto.FirstName) || !DoctorValidator.IsValidName(doctorDto.LastName))
            {
                throw new InvalidNameException(_messageService.GetMessage("InvalidNameException"));
            }

            // Validate Date of Birth
            if (!DoctorValidator.IsValidDOB(doctorDto.DOB))
            {
                throw new InvalidDOBException(_messageService.GetMessage("InvalidDOBException"));
            }

            // Validate Email
            if (!DoctorValidator.IsValidEmail(doctorDto.Email))
            {
                throw new InvalidEmailException(_messageService.GetMessage("InvalidEmailException"));
            }

            // Validate Gender
            if (!Enum.TryParse<Gender>(doctorDto.Gender.ToUpper(), out var gender))
            {
                throw new InvalidGenderException(_messageService.GetMessage("InvalidGenderException"));
            }

            // Validate Phone Number
            if (!DoctorValidator.IsValidPhoneNumber(doctorDto.PhoneNo))
            {
                throw new InvalidPhoneNumberException(_messageService.GetMessage("InvalidPhoneNumberException"));
            }

            byte[] imageBytes = null;
            // Validate profile picture
            if (doctorDto.profilePicture != null)
            {
                if (!DoctorValidator.IsValidImage(doctorDto.profilePicture))
                {
                    throw new InvalidImageTypeException(_messageService.GetMessage("InvalidImageTypeException"));
                }
                else if (!DoctorValidator.IsValidImageSize(doctorDto.profilePicture))
                {
                    throw new ImageFileSizeExceededException(_messageService.GetMessage("ImageFileSizeExceededException"));
                }
                else {
                    // Convert profile picture to byte array
                    if (doctorDto.profilePicture != null && doctorDto.profilePicture.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await doctorDto.profilePicture.CopyToAsync(memoryStream);
                            imageBytes = memoryStream.ToArray();
                        }
                    }
                }
            }

            // Validate Department
            if (!await _departmentRepository.IsValidDepartmentAsync(doctorDto.DepartmentId))
            {
                 throw new InvalidDepartmentException(_messageService.GetMessage("InvalidDepartmentException"));
            }

            // Validate Clinic
            if (!await _clinicRepository.IsValidClinicAsync(doctorDto.ClinicId))
            {
                 throw new InvalidClinicException(_messageService.GetMessage("InvalidClinicException"));
            }

            // Validate Address
            var clinicAddress = await _clinicRepository.GetClinicAddressAsync(doctorDto.ClinicId);
            if (clinicAddress == null || !await IsAddressMatchingClinicAsync(doctorDto.ClinicId, doctorDto.City, doctorDto.State, doctorDto.Country))
            {
                throw new InvalidDoctorAddressException(_messageService.GetMessage("InvalidDoctorAddressException"));
            }


            // Map Dto to doctor entity
            var doctor = new Doctor
            {
                FirstName = doctorDto.FirstName,
                LastName = doctorDto.LastName,
                BriefDescription = doctorDto.Biography,
                DateOfBirth = doctorDto.DOB,
                Email = doctorDto.Email,
                Gender = gender,
                Qualification = doctorDto.Qualification,
                ExperienceInYears = doctorDto.ExperienceInYears,
                Specialization = doctorDto.Specialization,
                ClinicId = doctorDto.ClinicId,
                DepartmentId = doctorDto.DepartmentId,
                PhoneNo = doctorDto.PhoneNo,
                ProfilePicture = imageBytes,
                CreatedAt = DateTime.Now,
                CreatedBy = "admin",
                LastModifiedBy = "admin",
                Status = doctorDto.Status,
                DoctorAddress = new DoctorAddress
                {
                    Street = doctorDto.Address,
                    City = doctorDto.City,
                    LastModifiedDate = DateTime.Now,
                    Country = doctorDto.Country,
                    ZipCode = doctorDto.ZipCode,
                    CreatedBy = "admin",
                    LastModifiedBy = "admin",
                    CreatedDate = DateTime.Now,
                    State = doctorDto.State
                }
            };

            await _doctorRepository.AddDoctorAsync(doctor);
            return doctor;
        }

        /// <summary>
        /// Edits an existing doctor in the system.
        /// </summary>
        /// <remarks>
        /// Validates the provided doctor details including name, date of birth, email, and phone number. Updates the existing <see cref="Doctor"/> entity with new details from the <see cref="DoctorDto"/>.
        /// </remarks>
        /// <param name="doctor">The <see cref="Doctor"/> entity to be updated.</param>
        /// <param name="doctorDto">The <see cref="DoctorDto"/> containing the updated details of the doctor.</param>
        /// <returns>
        /// The updated <see cref="Doctor"/> entity.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the input data is invalid, such as incorrect name format, invalid date of birth, email, or phone number.</exception>
        public async Task<Doctor> EditDoctorAsync(Doctor doctor, DoctorDto doctorDto)
        {
            // Validate Name
            if (!DoctorValidator.IsValidName(doctorDto.FirstName) || !DoctorValidator.IsValidName(doctorDto.LastName))
            {
                throw new InvalidNameException(_messageService.GetMessage("InvalidNameException"));
            }

            // Validate Date of Birth
            if (!DoctorValidator.IsValidDOB(doctorDto.DOB))
            {
                throw new InvalidDOBException(_messageService.GetMessage("InvalidDOBException"));
            }

            // Validate Email
            if (!DoctorValidator.IsValidEmail(doctorDto.Email))
            {
                throw new InvalidEmailException(_messageService.GetMessage("InvalidEmailException"));
            }

            // Validate Gender
            if (!Enum.TryParse<Gender>(doctorDto.Gender.ToUpper(), out var gender))
            {
                throw new InvalidGenderException(_messageService.GetMessage("InvalidGenderException"));
            }

            // Validate Phone Number
            if (!DoctorValidator.IsValidPhoneNumber(doctorDto.PhoneNo))
            {
                throw new InvalidPhoneNumberException(_messageService.GetMessage("InvalidPhoneNumberException"));
            }

            byte[] imageBytes = null;
            // Validate profile picture
            if (doctorDto.profilePicture != null)
            {
                if (!DoctorValidator.IsValidImage(doctorDto.profilePicture))
                {
                    throw new InvalidImageTypeException(_messageService.GetMessage("InvalidImageTypeException"));
                }
                else if (!DoctorValidator.IsValidImageSize(doctorDto.profilePicture))
                {
                    throw new ImageFileSizeExceededException(_messageService.GetMessage("ImageFileSizeExceededException"));
                }
                else
                {
                    // Convert profile picture to byte array
                    if (doctorDto.profilePicture != null && doctorDto.profilePicture.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await doctorDto.profilePicture.CopyToAsync(memoryStream);
                            imageBytes = memoryStream.ToArray();
                        }
                    }
                }
            }

            // Validate Department
            if (!await _departmentRepository.IsValidDepartmentAsync(doctorDto.DepartmentId))
            {
                throw new InvalidDepartmentException(_messageService.GetMessage("InvalidDepartmentException"));
            }

            // Validate Clinic
            if (!await _clinicRepository.IsValidClinicAsync(doctorDto.ClinicId))
            {
                throw new InvalidClinicException(_messageService.GetMessage("InvalidClinicException"));
            }

            // Validate Address
            var clinicAddress = await _clinicRepository.GetClinicAddressAsync(doctorDto.ClinicId);
            if (clinicAddress == null || !await IsAddressMatchingClinicAsync(doctorDto.ClinicId, doctorDto.City, doctorDto.State, doctorDto.Country))
            {
                throw new InvalidDoctorAddressException(_messageService.GetMessage("InvalidDoctorAddressException"));
            }

            // Map Dto to existing doctor
            doctor.FirstName = doctorDto.FirstName;
            doctor.LastName = doctorDto.LastName;
            doctor.BriefDescription = doctorDto.Biography;
            doctor.DateOfBirth = doctorDto.DOB;
            doctor.Email = doctorDto.Email;
            doctor.Gender = gender;
            doctor.ClinicId = doctorDto.ClinicId;
            doctor.DepartmentId = doctorDto.DepartmentId;
            doctor.PhoneNo = doctorDto.PhoneNo;
            doctor.Status = doctorDto.Status;
            doctor.ProfilePicture = imageBytes;
            doctor.Specialization = doctorDto.Specialization;
            doctor.Qualification = doctorDto.Qualification;
            doctor.ExperienceInYears = doctorDto.ExperienceInYears;
            doctor.LastModifiedBy = "admin";
            doctor.DoctorAddress.Street = doctorDto.Address;
            doctor.DoctorAddress.City = doctorDto.City;
            doctor.DoctorAddress.State = doctorDto.State;
            doctor.DoctorAddress.Country = doctorDto.Country;
            doctor.DoctorAddress.ZipCode = doctorDto.ZipCode;
            doctor.DoctorAddress.LastModifiedBy = "admin";
            doctor.DoctorAddress.LastModifiedDate = DateTime.Now;

            await _doctorRepository.EditDoctorAsync(doctor);
            return doctor;
        }

        /// <summary>
        /// Retrieves a paginated list of doctors.
        /// </summary>
        /// <remarks>
        /// Returns a paginated list of doctors with information about the current page, page size, total items, and total pages.
        /// </remarks>
        /// <param name="doctors">A list of <see cref="Doctor"/> entities to be paginated.</param>
        /// <param name="page">The page number to retrieve (must be greater than 0).</param>
        /// <param name="pageSize">The number of items per page (must be greater than 0).</param>
        /// <returns>
        /// An object containing the paginated list of doctors along with pagination metadata.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the page number or page size is less than or equal to 0.</exception>
        public async Task<object> GetAllDoctorAsync(List<Doctor> doctors, int page, int pageSize)
        {
            if (page <= 0) throw new InvalidPageNumberException(_messageService.GetMessage("InvalidPageNumberException"));
            if (pageSize <= 0) throw new InvalidPageSizeException(_messageService.GetMessage("InvalidPageSizeException"));

            var schedules = doctors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Map Doctor to DoctorDto
            var doctorDtos = schedules.Select(doctor => new DoctorDto
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                DOB = doctor.DateOfBirth,
                Email = doctor.Email,
                Gender = doctor.Gender.ToString(),
                Address = doctor.DoctorAddress.Street,
                Country = doctor.DoctorAddress.Country,
                City = doctor.DoctorAddress.City,
                ZipCode = doctor.DoctorAddress.ZipCode,
                State = doctor.DoctorAddress.State,
                Biography = doctor.BriefDescription,
                PhoneNo = doctor.PhoneNo,
                Status = doctor.Status,
                Specialization = doctor.Specialization,
                ExperienceInYears = doctor.ExperienceInYears,
                Qualification = doctor.Qualification,
                DepartmentId = doctor.DepartmentId,
                ClinicId = doctor.ClinicId,
                profilePicture = null 
            }).ToList();


            return new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = doctors.Count(),
                TotalPages = (int)Math.Ceiling((double)doctors.Count() / pageSize),
                Data = schedules
            };
        }

        /// <summary>
        /// Checks if the address provided matches the clinic's address.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic to compare with.</param>
        /// <param name="city">The city of the address.</param>
        /// <param name="state">The state of the address.</param>
        /// <param name="country">The country of the address.</param>
        /// <returns>A boolean indicating if the address matches the clinic's address.</returns>
        public async Task<bool> IsAddressMatchingClinicAsync(int clinicId, string city, string state, string country)
        {
            var clinic = await _clinicRepository.GetClinicAddressAsync(clinicId);
            if (clinic == null)
            {
                return false;
            }

            return clinic.Country == country; 

            /*return clinic.Address.City == city &&
                   clinic.Address.State == state &&
                   clinic.Address.Country == country;*/
        }
    }
}
