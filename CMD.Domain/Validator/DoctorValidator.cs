using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace CMD.Domain.Validator
{
    /// <summary>
    /// Provides validation methods for doctor-related data.
    /// </summary>
    public static class DoctorValidator
    {
        /// <summary>
        /// Validates the format of a name.
        /// </summary>
        /// <param name="name">The name to validate.</param>
        /// <returns>
        /// <c>true</c> if the name is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The name must be between 2 and 50 characters long, contain only letters and spaces, and must not start or end with a space.
        /// </remarks>
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2 || name.Length > 50)
                return false;

            // Ensure no numbers or special characters, no spaces at start or end
            return Regex.IsMatch(name, @"^[A-Za-z]+(?: [A-Za-z]+)*$");
        }

        /// <summary>
        /// Validates the date of birth of a doctor.
        /// </summary>
        /// <param name="dob">The date of birth to validate.</param>
        /// <returns>
        /// <c>true</c> if the date of birth is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The date of birth must be a past date, and the doctor must be at least 18 years old.
        /// </remarks>
        public static bool IsValidDOB(DateTime dob)
        {
            // Future date check
            if (dob > DateTime.Today)
                return false;

            int age = DateTime.Today.Year - dob.Year;
            if (dob > DateTime.Today.AddYears(-age)) age--;

            // Patients under 18 need a guardian
            if (age < 18)
                return false;

            return true;
        }

        /// <summary>
        /// Validates the format of an email address.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>
        /// <c>true</c> if the email is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The email must follow the format "local-part@domain" where both parts are non-empty and the domain contains at least one dot.
        /// </remarks>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Email validation regex
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        /// <summary>
        /// Validates the format of a phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <returns>
        /// <c>true</c> if the phone number is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The phone number must consist of digits only, optionally prefixed with a "+" sign, and have a length between 10 and 15 digits.
        /// </remarks>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Phone number should be digits only with a length between 10 and 15
            return Regex.IsMatch(phoneNumber, @"^\+?\d{10,15}$");
        }

        public static bool IsValidImage(IFormFile profilePicture)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(profilePicture.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return false;
            }
            return true;
        }
    }
}
