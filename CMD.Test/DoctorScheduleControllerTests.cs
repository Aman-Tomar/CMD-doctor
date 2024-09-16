using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CMD.API.Controllers;
using CMD.Domain.DTO;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using CMD.Domain.Managers;
using CMD.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMD.Test
{
    [TestClass]
    public class DoctorScheduleControllerTests
    {
        private Mock<IDoctorScheduleRepository> _doctorScheduleRepositoryMock;
        private Mock<IDoctorRepository> _doctorRepositoryMock;
        private Mock<IDoctorScheduleManager> _doctorScheduleManagerMock;
        private Mock<IMessageService> _messageServiceMock;
        private DoctorScheduleController _controller;

        /// <summary>
        /// Initializes test objects before each test is run.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _doctorScheduleRepositoryMock = new Mock<IDoctorScheduleRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _doctorScheduleManagerMock = new Mock<IDoctorScheduleManager>();
            _messageServiceMock = new Mock<IMessageService>();

            _controller = new DoctorScheduleController(
                _doctorScheduleRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _doctorScheduleManagerMock.Object,
                _messageServiceMock.Object
            );
        }

        /// <summary>
        /// Tests that the controller returns a BadRequest when the model state is invalid.
        /// </summary>
        [TestMethod]
        public async Task AddDoctorSchedule_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model state");

            // Act
            var result = await _controller.AddDoctorSchedule(1, new DoctorScheduleDto());

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// Tests that the controller returns a NotFound when the doctor is not found.
        /// </summary>
        [TestMethod]
        public async Task AddDoctorSchedule_ShouldReturnNotFound_WhenDoctorNotFound()
        {
            // Arrange
            _doctorRepositoryMock.Setup(repo => repo.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Doctor)null);

            _messageServiceMock.Setup(service => service.GetMessage("DoctorNotFound"))
                .Returns("Doctor not found");

            // Act
            var result = await _controller.AddDoctorSchedule(1, new DoctorScheduleDto());

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        /// <summary>
        /// Tests that the controller returns a Created result when the doctor schedule is created successfully.
        /// </summary>
        [TestMethod]
        public async Task AddDoctorSchedule_ShouldReturnCreated_WhenDoctorScheduleCreatedSuccessfully()
        {
            // Arrange
            var doctor = new Doctor { DoctorId = 1 };
            var doctorSchedule = new DoctorSchedule { DoctorScheduleId = 1 };

            _doctorRepositoryMock.Setup(repo => repo.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _doctorScheduleManagerMock.Setup(manager => manager.CreateDoctorScheduleAsync(It.IsAny<int>(), It.IsAny<DoctorScheduleDto>()))
                .ReturnsAsync(doctorSchedule);

            // Act
            var result = await _controller.AddDoctorSchedule(1, new DoctorScheduleDto());

            // Assert
            var createdResult = result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual($"api/DoctorSchedule/{doctorSchedule.DoctorScheduleId}", createdResult.Location);
        }

        /// <summary>
        /// Tests that the controller returns a NotFound when the doctor is not found during an edit operation.
        /// </summary>
        [TestMethod]
        public async Task EditDoctorSchedule_ShouldReturnNotFound_WhenDoctorNotFound()
        {
            // Arrange
            _doctorRepositoryMock.Setup(repo => repo.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Doctor)null);

            _messageServiceMock.Setup(service => service.GetMessage("DoctorNotFound"))
                .Returns("Doctor not found");

            // Act
            var result = await _controller.EditDoctorSchedule(1, new DoctorScheduleDto());

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        /// <summary>
        /// Tests that the controller returns a NotFound when the doctor schedule is not found during an edit operation.
        /// </summary>
        [TestMethod]
        public async Task EditDoctorSchedule_ShouldReturnNotFound_WhenDoctorScheduleNotFound()
        {
            // Arrange
            var doctor = new Doctor { DoctorId = 1 };

            _doctorRepositoryMock.Setup(repo => repo.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _doctorScheduleRepositoryMock.Setup(repo => repo.GetDoctorScheduleByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((DoctorSchedule)null);

            _messageServiceMock.Setup(service => service.GetMessage("DoctorScheduleNotFound"))
                .Returns("Doctor schedule not found");

            // Act
            var result = await _controller.EditDoctorSchedule(1, new DoctorScheduleDto());

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        /// <summary>
        /// Tests that the controller returns an Ok result when the doctor schedule is updated successfully.
        /// </summary>
        [TestMethod]
        public async Task EditDoctorSchedule_ShouldReturnOk_WhenDoctorScheduleUpdatedSuccessfully()
        {
            // Arrange
            var doctor = new Doctor { DoctorId = 1 };
            var doctorSchedule = new DoctorSchedule { DoctorScheduleId = 1 };

            _doctorRepositoryMock.Setup(repo => repo.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _doctorScheduleRepositoryMock.Setup(repo => repo.GetDoctorScheduleByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctorSchedule);

            _doctorScheduleManagerMock.Setup(manager => manager.EditDoctorScheduleAsync(It.IsAny<DoctorScheduleDto>(), It.IsAny<DoctorSchedule>(), It.IsAny<Doctor>()))
                .ReturnsAsync(doctorSchedule);

            // Act
            var result = await _controller.EditDoctorSchedule(1, new DoctorScheduleDto());

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        /// <summary>
        /// Tests that the controller returns a NotFound when the doctor is not found during a get operation.
        /// </summary>
        [TestMethod]
        public async Task GetDoctorSchedule_ShouldReturnNotFound_WhenDoctorNotFound()
        {
            // Arrange
            _doctorRepositoryMock.Setup(repo => repo.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Doctor)null);

            _messageServiceMock.Setup(service => service.GetMessage("DoctorNotFound"))
                .Returns("Doctor not found");

            // Act
            var result = await _controller.GetDoctorSchedule(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        /// <summary>
        /// Tests that the controller returns an Ok result when the doctor schedule is found.
        /// </summary>
        [TestMethod]
        public async Task GetDoctorSchedule_ShouldReturnOk_WhenDoctorScheduleFound()
        {
            // Arrange
            var doctor = new Doctor { DoctorId = 1 };
            var doctorSchedules = new List<DoctorSchedule> { new DoctorSchedule { DoctorScheduleId = 1 } };

            _doctorRepositoryMock.Setup(repo => repo.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _doctorScheduleRepositoryMock.Setup(repo => repo.GetScheduleByDoctorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctorSchedules);

            _doctorScheduleManagerMock.Setup(manager => manager.GetDoctorScheduleAsync(It.IsAny<List<DoctorSchedule>>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<DoctorSchedule> { new DoctorSchedule { DoctorScheduleId = 1 } });

            // Act
            var result = await _controller.GetDoctorSchedule(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        /// <summary>
        /// Tests that the controller returns NotFound when a doctor schedule is not found by ID.
        /// </summary>
        [TestMethod]
        public async Task GetScheduleByDoctorScheduleId_ShouldReturnNotFound_WhenDoctorScheduleNotFound()
        {
            // Arrange
            _doctorScheduleRepositoryMock.Setup(repo => repo.GetDoctorScheduleByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((DoctorSchedule)null);

            _messageServiceMock.Setup(service => service.GetMessage("DoctorScheduleNotFound"))
                .Returns("Doctor schedule not found");

            // Act
            var result = await _controller.GetScheduleByDoctorScheduleId(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        /// <summary>
        /// Tests that the controller returns Ok when a doctor schedule is found by ID.
        /// </summary>
        [TestMethod]
        public async Task GetScheduleByDoctorScheduleId_ShouldReturnOk_WhenDoctorScheduleFound()
        {
            // Arrange
            var doctorSchedule = new DoctorSchedule { DoctorScheduleId = 1 };
            _doctorScheduleRepositoryMock.Setup(repo => repo.GetDoctorScheduleByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctorSchedule);

            // Act
            var result = await _controller.GetScheduleByDoctorScheduleId(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        /// <summary>
        /// Tests that the controller returns NotFound when no doctor schedules are found for pagination.
        /// </summary>
        [TestMethod]
        public async Task GetAllSchedules_ShouldReturnNotFound_WhenNoDoctorSchedulesFound()
        {
            // Arrange
            _doctorScheduleRepositoryMock.Setup(repo => repo.GetAllSchedulesAsync())
                .ReturnsAsync(new List<DoctorSchedule>());

            _messageServiceMock.Setup(service => service.GetMessage("DoctorScheduleNotFound"))
                .Returns("Doctor schedule not found");

            // Act
            var result = await _controller.GetAllSchedules(1, 10);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        /// <summary>
        /// Tests that the controller returns Ok when doctor schedules are successfully retrieved with pagination.
        /// </summary>
        [TestMethod]
        public async Task GetAllSchedules_ShouldReturnOk_WhenDoctorSchedulesFound()
        {
            // Arrange
            var doctorSchedules = new List<DoctorSchedule> { new DoctorSchedule { DoctorScheduleId = 1 } };
            _doctorScheduleRepositoryMock.Setup(repo => repo.GetAllSchedulesAsync())
                .ReturnsAsync(doctorSchedules);

            _doctorScheduleManagerMock.Setup(manager => manager.GetDoctorScheduleAsync(It.IsAny<List<DoctorSchedule>>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(doctorSchedules);

            // Act
            var result = await _controller.GetAllSchedules(1, 10);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        /// <summary>
        /// Tests that the controller returns NotFound when a doctor is not found during schedule check.
        /// </summary>
        [TestMethod]
        public async Task CheckDoctorSchedule_ShouldReturnNotFound_WhenDoctorNotFound()
        {
            // Arrange
            _doctorRepositoryMock.Setup(repo => repo.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Doctor)null);

            _messageServiceMock.Setup(service => service.GetMessage("DoctorNotFound"))
                .Returns("Doctor not found");

            // Act
            var result = await _controller.CheckDoctorSchedule(1, DateOnly.FromDateTime(DateTime.Now), TimeOnly.MinValue, TimeOnly.MaxValue);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        /// <summary>
        /// Tests that the controller returns Ok when the doctor's schedule exists on a specific date.
        /// </summary>
        [TestMethod]
        public async Task CheckDoctorSchedule_ShouldReturnOk_WhenScheduleExists()
        {
            // Arrange
            var doctor = new Doctor { DoctorId = 1 };
            _doctorRepositoryMock.Setup(repo => repo.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _doctorScheduleRepositoryMock.Setup(repo => repo.DoesScheduleExistAsync(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CheckDoctorSchedule(1, DateOnly.FromDateTime(DateTime.Now), TimeOnly.MinValue, TimeOnly.MaxValue);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

    }
}
