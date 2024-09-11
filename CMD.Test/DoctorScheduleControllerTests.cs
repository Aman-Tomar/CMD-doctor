using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CMD.API.Controllers;
using CMD.Domain.Entities;
using CMD.Domain.Repositories;
using CMD.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CMD.Test
{
    [TestClass]
    public class DoctorScheduleControllerTests
    {
        private Mock<IDoctorScheduleRepository> _mockDoctorScheduleRepository;
        private Mock<IDoctorRepository> _mockDoctorRepository;
        private Mock<IManageDoctorSchedule> _mockManageDoctorSchedule;
        private DoctorScheduleController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockDoctorScheduleRepository = new Mock<IDoctorScheduleRepository>();
            _mockDoctorRepository = new Mock<IDoctorRepository>();
            _mockManageDoctorSchedule = new Mock<IManageDoctorSchedule>();
            _controller = new DoctorScheduleController(
                _mockDoctorScheduleRepository.Object,
                _mockDoctorRepository.Object,
                _mockManageDoctorSchedule.Object
            );
        }

        [TestMethod]
        public async Task AddDoctorSchedule_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");

            // Act
            var result = await _controller.AddDoctorSchedule(1, new DoctorSchedule());

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task AddDoctorSchedule_DoctorNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockDoctorRepository
                .Setup(repo => repo.GetDoctorById(It.IsAny<int>()))
                .ReturnsAsync((Doctor)null);

            // Act
            var result = await _controller.AddDoctorSchedule(1, new DoctorSchedule());

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.AreEqual("Doctor Not Found", notFoundResult.Value);
        }

        [TestMethod]
        public async Task AddDoctorSchedule_OverlappingSchedule_ReturnsBadRequest()
        {
            // Arrange
            var existingSchedule = new DoctorSchedule
            {
                StartTime = TimeOnly.FromDateTime(new DateTime(2024, 9, 11, 10, 0, 0)),
                EndTime = TimeOnly.FromDateTime(new DateTime(2024, 9, 11, 11, 0, 0))
            };
            _mockDoctorRepository
                .Setup(repo => repo.GetDoctorById(It.IsAny<int>()))
                .ReturnsAsync(new Doctor());
            _mockManageDoctorSchedule
                .Setup(service => service.IsAvailable(It.IsAny<DoctorSchedule>()))
                .ReturnsAsync(false);

            var newSchedule = new DoctorSchedule
            {
                StartTime = TimeOnly.FromDateTime(new DateTime(2024, 9, 11, 10, 30, 0)),
                EndTime = TimeOnly.FromDateTime(new DateTime(2024, 9, 11, 11, 30, 0))
            };

            // Act
            var result = await _controller.AddDoctorSchedule(1, newSchedule);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.AreEqual("Cannot create schedule for time slot because a schedule already exist.", badRequestResult.Value);
        }
    }
}
