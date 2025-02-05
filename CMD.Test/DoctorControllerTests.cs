﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CMD.API.Controllers;
using CMD.Domain.DTO;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using CMD.Domain.Managers;
using CMD.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CMD.Test
{
    [TestClass]
    public class DoctorControllerTests
    {
        private Mock<IDoctorRepository> _doctorRepositoryMock;
        private Mock<IDoctorManager> _doctorManagerMock;
        private Mock<IMessageService> _messageServiceMock;
        private DoctorController _controller;

        /// <summary>
        /// Initializes the test dependencies and controller before each test case.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _doctorManagerMock = new Mock<IDoctorManager>();
            _messageServiceMock = new Mock<IMessageService>();
            _controller = new DoctorController(
                _doctorRepositoryMock.Object,
                _doctorManagerMock.Object,
                _messageServiceMock.Object);
        }

        /// <summary>
        /// Tests that a valid doctor is successfully added and returns a 201 Created result.
        /// </summary>
        [TestMethod]
        public async Task AddDoctor_ValidDoctor_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var doctorDto = new DoctorDto();
            var createdDoctor = new Doctor { DoctorId = 1 };

            _doctorManagerMock
                .Setup(dm => dm.AddDoctorAsync(It.IsAny<DoctorDto>()))
                .ReturnsAsync(createdDoctor);

            // Act
            var result = await _controller.AddDoctor(doctorDto);

            // Assert
            var actionResult = result as CreatedResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(201, actionResult.StatusCode);
            Assert.AreEqual($"api/Doctor/{createdDoctor.DoctorId}", actionResult.Location);
        }

        /// <summary>
        /// Tests that a doctor with an invalid model state returns a 400 Bad Request result.
        /// </summary>
        [TestMethod]
        public async Task AddDoctor_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");

            // Act
            var result = await _controller.AddDoctor(new DoctorDto());

            // Assert
            var actionResult = result as BadRequestObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(400, actionResult.StatusCode);
        }

        /// <summary>
        /// Tests that editing an existing doctor returns a 200 OK result.
        /// </summary>
        [TestMethod]
        public async Task EditDoctor_ValidDoctor_ReturnsOkResult()
        {
            // Arrange
            var doctorId = 1;
            var doctorDto = new DoctorDto();
            var existingDoctor = new Doctor { DoctorId = doctorId };
            var updatedDoctor = new Doctor();

            _doctorRepositoryMock
                .Setup(repo => repo.GetDoctorByIdAsync(doctorId))
                .ReturnsAsync(existingDoctor);

            _doctorManagerMock
                .Setup(dm => dm.EditDoctorAsync(existingDoctor, doctorDto))
                .ReturnsAsync(updatedDoctor);

            // Act
            var result = await _controller.EditDoctor(doctorId, doctorDto);

            // Assert
            var actionResult = result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(200, actionResult.StatusCode);
        }

        /// <summary>
        /// Tests that trying to edit a doctor that doesn't exist returns a 404 Not Found result.
        /// </summary>
        [TestMethod]
        public async Task EditDoctor_DoctorNotFound_ReturnsNotFound()
        {
            // Arrange
            var doctorId = 1;
            var doctorDto = new DoctorDto();

            _doctorRepositoryMock
                .Setup(repo => repo.GetDoctorByIdAsync(doctorId))
                .ReturnsAsync((Doctor)null);

            _messageServiceMock
                .Setup(ms => ms.GetMessage("DoctorNotFound"))
                .Returns("Doctor not found");

            // Act
            var result = await _controller.EditDoctor(doctorId, doctorDto);

            // Assert
            var actionResult = result as NotFoundObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(404, actionResult.StatusCode);
            Assert.AreEqual("Doctor not found", actionResult.Value);
        }

        /// <summary>
        /// Tests that retrieving all doctors returns a 200 OK result when doctors are found.
        /// </summary>
        [TestMethod]
        public async Task GetAllDoctors_DoctorsFound_ReturnsOkResult()
        {
            // Arrange
            var doctors = new List<Doctor> { new Doctor { DoctorId = 1 } };

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllDoctorsAsync())
                .ReturnsAsync(doctors);

            _doctorManagerMock
                .Setup(dm => dm.GetAllDoctorAsync(It.IsAny<List<Doctor>>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new { });

            // Act
            var result = await _controller.GetAllDoctors();

            // Assert
            var actionResult = result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(200, actionResult.StatusCode);
        }

        /// <summary>
        /// Tests that retrieving all doctors returns a 404 Not Found result when no doctors are found.
        /// </summary>
        [TestMethod]
        public async Task GetAllDoctors_NoDoctorsFound_ReturnsNotFound()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(repo => repo.GetAllDoctorsAsync())
                .ReturnsAsync((List<Doctor>)null);

            _messageServiceMock
                .Setup(ms => ms.GetMessage("DoctorNotFound"))
                .Returns("No doctors found");

            // Act
            var result = await _controller.GetAllDoctors();

            // Assert
            var actionResult = result as NotFoundObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(404, actionResult.StatusCode);
            Assert.AreEqual("No doctors found", actionResult.Value);
        }

        /// <summary>
        /// Tests that retrieving a doctor by ID returns a 200 OK result when the doctor is found.
        /// </summary>
        [TestMethod]
        public async Task GetDoctorById_DoctorFound_ReturnsOkResult()
        {
            // Arrange
            var doctorId = 1;
            var doctor = new Doctor { DoctorId = doctorId };

            _doctorRepositoryMock
                .Setup(repo => repo.GetDoctorByIdAsync(doctorId))
                .ReturnsAsync(doctor);

            // Act
            var result = await _controller.GetDoctorById(doctorId);

            // Assert
            var actionResult = result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(200, actionResult.StatusCode);
        }

        /// <summary>
        /// Tests that retrieving a doctor by ID returns a 404 Not Found result when the doctor is not found.
        /// </summary>
        [TestMethod]
        public async Task GetDoctorById_DoctorNotFound_ReturnsNotFound()
        {
            // Arrange
            var doctorId = 1;

            _doctorRepositoryMock
                .Setup(repo => repo.GetDoctorByIdAsync(doctorId))
                .ReturnsAsync((Doctor)null);

            _messageServiceMock
                .Setup(ms => ms.GetMessage("DoctorNotFound"))
                .Returns("Doctor not found");

            // Act
            var result = await _controller.GetDoctorById(doctorId);

            // Assert
            var actionResult = result as NotFoundObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(404, actionResult.StatusCode);
            Assert.AreEqual("Doctor not found", actionResult.Value);
        }
    }
}
