using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CMD.Domain.Managers;
using CMD.Domain.Repositories;
using CMD.Domain.Services;
using CMD.Domain.Entities;
using CMD.Domain.DTO;
using CMD.Domain.Enums;
using CMD.Domain.Exceptions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CMD.Test
{
    [TestClass]
    public class DoctorScheduleManagerTests
    {
        private Mock<IDoctorScheduleRepository> _mockDoctorScheduleRepository;
        private Mock<IDoctorRepository> _mockDoctorRepository;
        private Mock<IMessageService> _mockMessageService;
        private DoctorScheduleManager _doctorScheduleManager;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockDoctorScheduleRepository = new Mock<IDoctorScheduleRepository>();
            _mockDoctorRepository = new Mock<IDoctorRepository>();
            _mockMessageService = new Mock<IMessageService>();

            _doctorScheduleManager = new DoctorScheduleManager(_mockDoctorScheduleRepository.Object, _mockDoctorRepository.Object, _mockMessageService.Object);
        }

        /// <summary>
        /// Tests that a valid doctor schedule creation returns a DoctorSchedule object.
        /// </summary>
        [TestMethod]
        public async Task CreateDoctorScheduleAsync_ValidInput_ShouldReturnDoctorSchedule()
        {
            // Arrange
            var doctorScheduleDto = new DoctorScheduleDto
            {
                ClinicId = 1,
                Weekday = "MONDAY",
                StartTime = "09:00",
                EndTime = "17:00",
                Status = true,
                DoctorId = 1
            };

            _mockDoctorScheduleRepository.Setup(repo => repo.CreateDoctorScheduleAsync(It.IsAny<DoctorSchedule>()))
                                         .Returns(Task.CompletedTask);

            _mockDoctorScheduleRepository.Setup(repo => repo.GetDoctorScheduleForWeekdayAsync(It.IsAny<int>(), It.IsAny<Weekday>()))
                                         .ReturnsAsync(new List<DoctorSchedule>());

            _mockMessageService.Setup(service => service.GetMessage(It.IsAny<string>())).Returns("Error");

            // Act
            var result = await _doctorScheduleManager.CreateDoctorScheduleAsync(1, doctorScheduleDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ClinicId);
            Assert.AreEqual(Weekday.MONDAY, result.Weekday);
            Assert.AreEqual(TimeOnly.Parse("09:00"), result.StartTime);
            Assert.AreEqual(TimeOnly.Parse("17:00"), result.EndTime);
        }

        /// <summary>
        /// Tests that creating a doctor schedule with an invalid weekday throws an InvalidWeekdayException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidWeekdayException))]
        public async Task CreateDoctorScheduleAsync_InvalidWeekday_ShouldThrowInvalidWeekdayException()
        {
            // Arrange
            var doctorScheduleDto = new DoctorScheduleDto
            {
                ClinicId = 1,
                Weekday = "INVALID",
                StartTime = "09:00",
                EndTime = "17:00",
                Status = true,
                DoctorId = 1
            };

            _mockMessageService.Setup(service => service.GetMessage(It.IsAny<string>())).Returns("Invalid Weekday");

            // Act
            await _doctorScheduleManager.CreateDoctorScheduleAsync(1, doctorScheduleDto);

            // Assert: Exception is expected
        }

        /// <summary>
        /// Tests that creating a doctor schedule with an invalid time range throws an InvalidTimeException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidTimeException))]
        public async Task CreateDoctorScheduleAsync_InvalidTime_ShouldThrowInvalidTimeException()
        {
            // Arrange
            var doctorScheduleDto = new DoctorScheduleDto
            {
                ClinicId = 1,
                Weekday = "MONDAY",
                StartTime = "17:00",
                EndTime = "09:00", // Invalid time
                Status = true,
                DoctorId = 1
            };

            _mockMessageService.Setup(service => service.GetMessage(It.IsAny<string>())).Returns("Invalid Time");
            _mockDoctorScheduleRepository.Setup(repo => repo.GetDoctorScheduleForWeekdayAsync(It.IsAny<int>(), It.IsAny<Weekday>()))
                                         .ReturnsAsync(new List<DoctorSchedule>());

            // Act
            await _doctorScheduleManager.CreateDoctorScheduleAsync(1, doctorScheduleDto);

            // Assert: Exception is expected
        }

        /// <summary>
        /// Tests that creating a doctor schedule when the doctor is not available throws an InvalidDoctorScheduleException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidDoctorScheduleException))]
        public async Task CreateDoctorScheduleAsync_DoctorNotAvailable_ShouldThrowInvalidDoctorScheduleException()
        {
            // Arrange
            var doctorScheduleDto = new DoctorScheduleDto
            {
                ClinicId = 1,
                Weekday = "MONDAY",
                StartTime = "09:00",
                EndTime = "17:00",
                Status = true,
                DoctorId = 1
            };

            var existingSchedules = new List<DoctorSchedule>
            {
                new DoctorSchedule
                {
                    StartTime = TimeOnly.Parse("08:00"),
                    EndTime = TimeOnly.Parse("10:00"),
                }
            };

            _mockMessageService.Setup(service => service.GetMessage(It.IsAny<string>())).Returns("Doctor not available");
            _mockDoctorScheduleRepository.Setup(repo => repo.GetDoctorScheduleForWeekdayAsync(It.IsAny<int>(), It.IsAny<Weekday>()))
                                         .ReturnsAsync(existingSchedules);

            // Act
            await _doctorScheduleManager.CreateDoctorScheduleAsync(1, doctorScheduleDto);

            // Assert: Exception is expected
        }

        /// <summary>
        /// Tests that the schedule time validity check correctly identifies an invalid time range.
        /// </summary>
        [TestMethod]
        public async Task IsScheduleTimeValidAsync_ValidTime_ShouldReturnFalse()
        {
            // Act
            var result = await _doctorScheduleManager.IsScheduleTimeValidAsync("09:00", "17:00");

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests that retrieving doctor schedules returns a paginated result.
        /// </summary>
        [TestMethod]
        public async Task GetDoctorScheduleAsync_ValidInput_ShouldReturnPaginatedResult()
        {
            // Arrange
            var schedules = new List<DoctorSchedule>
            {
                new DoctorSchedule { ClinicId = 1, DoctorId = 1, Weekday = Weekday.MONDAY, StartTime = TimeOnly.Parse("09:00"), EndTime = TimeOnly.Parse("17:00") },
                new DoctorSchedule { ClinicId = 1, DoctorId = 1, Weekday = Weekday.TUESDAY, StartTime = TimeOnly.Parse("09:00"), EndTime = TimeOnly.Parse("17:00") }
            };

            // Act
            var result = await _doctorScheduleManager.GetDoctorScheduleAsync(schedules, 1, 1);

            // Assert
            Assert.IsNotNull(result);
        }
        /// <summary>
        /// Tests that the doctor is available when there are no overlapping schedules.
        /// </summary>
        [TestMethod]
        public async Task IsDoctorAvailableForScheduleAsync_NoOverlap_ShouldReturnTrue()
        {
            // Arrange
            int doctorId = 1;
            Weekday weekday = Weekday.MONDAY;
            string startTime = "09:00";
            string endTime = "10:00";
            var existingSchedules = new List<DoctorSchedule>();

            _mockDoctorScheduleRepository.Setup(repo => repo.GetDoctorScheduleForWeekdayAsync(doctorId, weekday))
                                         .ReturnsAsync(existingSchedules);

            // Act
            var result = await _doctorScheduleManager.IsDoctorAvailableForScheduleAsync(doctorId, weekday, startTime, endTime);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests that the doctor is not available when there is an overlapping schedule.
        /// </summary>
        [TestMethod]
        public async Task IsDoctorAvailableForScheduleAsync_Overlap_ShouldReturnFalse()
        {
            // Arrange
            int doctorId = 1;
            Weekday weekday = Weekday.MONDAY;
            string startTime = "09:00";
            string endTime = "11:00";

            var existingSchedules = new List<DoctorSchedule>
            {
                new DoctorSchedule
                {
                    StartTime = TimeOnly.Parse("08:00"),
                    EndTime = TimeOnly.Parse("10:00")
                }
            };

            _mockDoctorScheduleRepository.Setup(repo => repo.GetDoctorScheduleForWeekdayAsync(doctorId, weekday))
                                         .ReturnsAsync(existingSchedules);

            // Act
            var result = await _doctorScheduleManager.IsDoctorAvailableForScheduleAsync(doctorId, weekday, startTime, endTime);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests that the doctor is available when the new schedule starts after all existing schedules.
        /// </summary>
        [TestMethod]
        public async Task IsDoctorAvailableForScheduleAsync_StartsAfterExistingSchedules_ShouldReturnTrue()
        {
            // Arrange
            int doctorId = 1;
            Weekday weekday = Weekday.MONDAY;
            string startTime = "12:00";
            string endTime = "13:00";

            var existingSchedules = new List<DoctorSchedule>
            {
                new DoctorSchedule
                {
                    StartTime = TimeOnly.Parse("08:00"),
                    EndTime = TimeOnly.Parse("11:00")
                }
            };

            _mockDoctorScheduleRepository.Setup(repo => repo.GetDoctorScheduleForWeekdayAsync(doctorId, weekday))
                                         .ReturnsAsync(existingSchedules);

            // Act
            var result = await _doctorScheduleManager.IsDoctorAvailableForScheduleAsync(doctorId, weekday, startTime, endTime);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests that the doctor is available when the new schedule ends before all existing schedules.
        /// </summary>
        [TestMethod]
        public async Task IsDoctorAvailableForScheduleAsync_EndsBeforeExistingSchedules_ShouldReturnTrue()
        {
            // Arrange
            int doctorId = 1;
            Weekday weekday = Weekday.MONDAY;
            string startTime = "07:00";
            string endTime = "08:00";

            var existingSchedules = new List<DoctorSchedule>
            {
                new DoctorSchedule
                {
                    StartTime = TimeOnly.Parse("08:00"),
                    EndTime = TimeOnly.Parse("10:00")
                }
            };

            _mockDoctorScheduleRepository.Setup(repo => repo.GetDoctorScheduleForWeekdayAsync(doctorId, weekday))
                                         .ReturnsAsync(existingSchedules);

            // Act
            var result = await _doctorScheduleManager.IsDoctorAvailableForScheduleAsync(doctorId, weekday, startTime, endTime);

            // Assert
            Assert.IsTrue(result);
        }

    }
}
