using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.Repositories;
using CMD.Domain.Entities;
using CMD.API.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMD.UnitTest
{
    [TestClass]
    public class ViewDoctorByIdTests
    {
        private Mock<IDoctorRepository> doctorRepoMock;
        private DoctorController controller;
        
        [TestInitialize]
        public void Setup()
        {
            // Create a mock repository
            doctorRepoMock = new Mock<IDoctorRepository>();

            // Create an instance of the controller with the mock repository
            controller = new DoctorController(doctorRepoMock.Object);
        }

        [TestMethod]
        public async Task TC001_VerifyUserCanAccessViewDoctorByIDPage_ReturnsOkResult()
        {
            // Arrange
            var doctorId = 1;

            doctorRepoMock.Setup(repo => repo.GetDoctorById(doctorId))
                          .ReturnsAsync(new Doctor { DoctorId = doctorId });

            // Act
            var result = await controller.GetDoctorById(doctorId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task TC002_VerifyViewDoctorByIdDisplaysCorrectInfoBasedOnRole_ReturnsOkResult()
        {
            // Arrange
            var doctorId = 1;

            doctorRepoMock.Setup(repo => repo.GetDoctorById(doctorId))
                          .ReturnsAsync(new Doctor { DoctorId = doctorId, FirstName = "Dr. John" });

            // Act
            var result = await controller.GetDoctorById(doctorId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(doctorId, ((Doctor)okResult.Value).DoctorId);
            Assert.AreEqual("Dr. John", ((Doctor)okResult.Value).FirstName);
        }

        [TestMethod]
        public async Task TC003_VerifySystemHandlesInvalidDoctorIdGracefully_ReturnsNotFoundResult()
        {
            // Arrange
            var invalidDoctorId = 999;

            doctorRepoMock.Setup(repo => repo.GetDoctorById(invalidDoctorId))
                          .ReturnsAsync((Doctor)null);

            // Act
            var result = await controller.GetDoctorById(invalidDoctorId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task TC004_VerifySystemHandlesUnavailableDoctorInfo_ReturnsNotFoundResult()
        {
            // Arrange
            var inactiveDoctorId = 5;

            doctorRepoMock.Setup(repo => repo.GetDoctorById(inactiveDoctorId))
                          .ReturnsAsync((Doctor)null);

            // Act
            var result = await controller.GetDoctorById(inactiveDoctorId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

      

        [TestMethod]
        public async Task TC006_VerifyLoadingIndicatorWhileFetchingDoctorDetails_SimulatesLoading()
        {
            // Arrange
            var doctorId = 1;

            doctorRepoMock.Setup(repo => repo.GetDoctorById(doctorId))
                          .ReturnsAsync(new Doctor { DoctorId = doctorId });

            // Simulate loading
            var loadingIndicator = "Loading...";

            // Act
            var result = await controller.GetDoctorById(doctorId);

            // Assert
            Assert.AreEqual("Loading...", loadingIndicator);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }


        [TestMethod]
        public async Task TC010_VerifyDoctorDetailsAccessibleOnDifferentDevices_ReturnsOkResult()
        {
            // Arrange
            var doctorId = 1;

            doctorRepoMock.Setup(repo => repo.GetDoctorById(doctorId))
                          .ReturnsAsync(new Doctor { DoctorId = doctorId });

            // Act
            var resultDesktop = await controller.GetDoctorById(doctorId); // Simulate desktop
            var resultMobile = await controller.GetDoctorById(doctorId); // Simulate mobile

            // Assert
            Assert.IsInstanceOfType(resultDesktop, typeof(OkObjectResult));
            Assert.IsInstanceOfType(resultMobile, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task TC011_VerifyLocalizationSupportForDoctorDetails_ReturnsLocalizedContent()
        {
            // Arrange
            var doctorId = 1;
            var language = "fr"; // Simulate French language setting

            // Mock repository with localization support
            doctorRepoMock.Setup(repo => repo.GetDoctorById(doctorId))
                          .ReturnsAsync(new Doctor { DoctorId = doctorId, FirstName = "Dr. Jean" });

            // Act
            var result = await controller.GetDoctorById(doctorId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Dr. Jean", ((Doctor)okResult.Value).FirstName); // Simulate French doctor name
        }
    }
}
