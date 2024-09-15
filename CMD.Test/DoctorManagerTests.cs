using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMD.Domain.DTO;
using CMD.Domain.Entities;
using CMD.Domain.Enums;
using CMD.Domain.Exceptions;
using CMD.Domain.Managers;
using CMD.Domain.Repositories;
using CMD.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CMD.Test
{
    /// <summary>
    /// Unit tests for the <see cref="DoctorManager"/> class.
    /// </summary>
    [TestClass]
    public class DoctorManagerTests
    {
        private Mock<IDoctorRepository> _doctorRepositoryMock;
        private Mock<IDepartmentRepository> _departmentRepositoryMock;
        private Mock<IClinicRepository> _clinicRepositoryMock;
        private Mock<IMessageService> _messageServiceMock;
        private DoctorManager _doctorManager;

        /// <summary>
        /// Initializes the required dependencies before each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _departmentRepositoryMock = new Mock<IDepartmentRepository>();
            _clinicRepositoryMock = new Mock<IClinicRepository>();
            _messageServiceMock = new Mock<IMessageService>();

            _doctorManager = new DoctorManager(
                _doctorRepositoryMock.Object,
                _departmentRepositoryMock.Object,
                _clinicRepositoryMock.Object,
                _messageServiceMock.Object);
        }

        /// <summary>
        /// Verifies that a valid doctor DTO is successfully added, and the doctor is returned.
        /// </summary>
        [TestMethod]
        public async Task AddDoctorAsync_ValidDoctorDto_ReturnsDoctor()
        {
            // Arrange
            var doctorDto = new DoctorDto
            {
                FirstName = "John",
                LastName = "Doe",
                DOB = new DateTime(1980, 1, 1),
                Email = "john.doe@example.com",
                PhoneNo = "1234567890",
                Gender = "Male",
                ClinicId = 1,
                DepartmentId = 1
            };

            _departmentRepositoryMock.Setup(repo => repo.IsValidDepartmentAsync(1)).ReturnsAsync(true);
            _clinicRepositoryMock.Setup(repo => repo.IsValidClinicAsync(1)).ReturnsAsync(true);
            _clinicRepositoryMock.Setup(repo => repo.GetClinicAddressAsync(1)).ReturnsAsync(new ClinicAddressDto());

            // Act
            var result = await _doctorManager.AddDoctorAsync(doctorDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("John", result.FirstName);
            _doctorRepositoryMock.Verify(repo => repo.AddDoctorAsync(It.IsAny<Doctor>()), Times.Once);
        }

        /// <summary>
        /// Verifies that an invalid doctor name throws an <see cref="InvalidNameException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public async Task AddDoctorAsync_InvalidName_ThrowsInvalidNameException()
        {
            // Arrange
            var doctorDto = new DoctorDto
            {
                FirstName = "John123", // Invalid name format
                LastName = "Doe",
                DOB = new DateTime(1980, 1, 1),
                Email = "john.doe@example.com",
                PhoneNo = "1234567890",
                Gender = "Male",
                ClinicId = 1,
                DepartmentId = 1
            };

            _messageServiceMock.Setup(service => service.GetMessage("InvalidNameException"))
                               .Returns("Invalid name format");

            // Act
            await _doctorManager.AddDoctorAsync(doctorDto);

            // Assert is handled by the ExpectedException attribute
        }

        /// <summary>
        /// Verifies that an invalid doctor email throws an <see cref="InvalidEmailException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidEmailException))]
        public async Task AddDoctorAsync_InvalidEmail_ThrowsInvalidEmailException()
        {
            // Arrange
            var doctorDto = new DoctorDto
            {
                FirstName = "John",
                LastName = "Doe",
                DOB = new DateTime(1980, 1, 1),
                Email = "invalid-email", // Invalid email format
                PhoneNo = "1234567890",
                Gender = "Male",
                ClinicId = 1,
                DepartmentId = 1
            };

            _messageServiceMock.Setup(service => service.GetMessage("InvalidEmailException"))
                               .Returns("Invalid email format");

            // Act
            await _doctorManager.AddDoctorAsync(doctorDto);

            // Assert is handled by the ExpectedException attribute
        }

        /// <summary>
        /// Verifies that providing an invalid page number when retrieving doctors throws an <see cref="InvalidPageNumberException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidPageNumberException))]
        public async Task GetAllDoctorAsync_InvalidPageNumber_ThrowsInvalidPageNumberException()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor { FirstName = "John", LastName = "Doe" },
                new Doctor { FirstName = "Jane", LastName = "Doe" }
            };

            int invalidPage = 0; // Invalid page number

            _messageServiceMock.Setup(service => service.GetMessage("InvalidPageNumberException"))
                               .Returns("Invalid page number");

            // Act
            await _doctorManager.GetAllDoctorAsync(doctors, invalidPage, 2);
        }

        /// <summary>
        /// Verifies that providing an invalid page size when retrieving doctors throws an <see cref="InvalidPageSizeException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidPageSizeException))]
        public async Task GetAllDoctorAsync_InvalidPageSize_ThrowsInvalidPageSizeException()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor { FirstName = "John", LastName = "Doe" },
                new Doctor { FirstName = "Jane", LastName = "Doe" }
            };

            int invalidPageSize = 0; // Invalid page size

            _messageServiceMock.Setup(service => service.GetMessage("InvalidPageSizeException"))
                               .Returns("Invalid page size");

            // Act
            await _doctorManager.GetAllDoctorAsync(doctors, 1, invalidPageSize);
        }
    }
}
