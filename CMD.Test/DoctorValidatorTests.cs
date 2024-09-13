using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.Validator;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CMD.Test
{
    [TestClass]
    public class DoctorValidatorTests
    {
        [TestMethod]
        public void IsValidName_ValidName_ReturnsTrue()
        {
            // Arrange
            var name = "John Doe";

            // Act
            var result = DoctorValidator.IsValidName(name);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidName_InvalidName_ReturnsFalse()
        {
            // Arrange
            var name = "J!";

            // Act
            var result = DoctorValidator.IsValidName(name);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidDOB_ValidDOB_ReturnsTrue()
        {
            // Arrange
            var dob = DateTime.Today.AddYears(-30);

            // Act
            var result = DoctorValidator.IsValidDOB(dob);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidDOB_FutureDOB_ReturnsFalse()
        {
            // Arrange
            var dob = DateTime.Today.AddYears(1);

            // Act
            var result = DoctorValidator.IsValidDOB(dob);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidDOB_Under18DOB_ReturnsFalse()
        {
            // Arrange
            var dob = DateTime.Today.AddYears(-17);

            // Act
            var result = DoctorValidator.IsValidDOB(dob);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            var result = DoctorValidator.IsValidEmail(email);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            var email = "invalid-email";

            // Act
            var result = DoctorValidator.IsValidEmail(email);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPhoneNumber_ValidPhoneNumber_ReturnsTrue()
        {
            // Arrange
            var phoneNumber = "+12345678901";

            // Act
            var result = DoctorValidator.IsValidPhoneNumber(phoneNumber);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidPhoneNumber_InvalidPhoneNumber_ReturnsFalse()
        {
            // Arrange
            var phoneNumber = "123-456";

            // Act
            var result = DoctorValidator.IsValidPhoneNumber(phoneNumber);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidImage_ValidImage_ReturnsTrue()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("image.jpg");

            // Act
            var result = DoctorValidator.IsValidImage(mockFile.Object);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidImage_InvalidImage_ReturnsFalse()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("document.pdf");

            // Act
            var result = DoctorValidator.IsValidImage(mockFile.Object);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidImageSize_ValidSize_ReturnsTrue()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(4 * 1024 * 1024); // 4 MB

            // Act
            var result = DoctorValidator.IsValidImageSize(mockFile.Object);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidImageSize_InvalidSize_ReturnsFalse()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(6 * 1024 * 1024); // 6 MB

            // Act
            var result = DoctorValidator.IsValidImageSize(mockFile.Object);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
