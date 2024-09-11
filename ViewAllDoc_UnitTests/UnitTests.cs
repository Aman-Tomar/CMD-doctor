using CMD.API.Controllers;
using CMD.Domain.DTO;
using CMD.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ViewAllDoc_UnitTests
{
    [TestClass]
    public class DoctorControllerUnitTest
    {
        private Mock<IDoctorRepository> _doctorRepoMock;
        private DoctorController _controller;

        [TestInitialize]
        public void Setup()
        {
            _doctorRepoMock = new Mock<IDoctorRepository>();
            _controller = new DoctorController(_doctorRepoMock.Object);
        }

        [TestMethod]
        public async Task TC001_VerifyListOfDoctors()
        {
            // Arrange
            var doctors = new List<DoctorDto>
        {
            new DoctorDto { FirstName = "John", LastName = "Doe", Specialization = "Cardiology", City = "New York" },
            new DoctorDto { FirstName = "Jane", LastName = "Smith", Specialization = "Neurology", City = "Chicago" }
        };
            _doctorRepoMock.Setup(repo => repo.GetAllDoctorsAsync(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(doctors);
            _doctorRepoMock.Setup(repo => repo.GetTotalNumberOfDoctorsAsync())
                           .ReturnsAsync(doctors.Count);

            // Act
            var actionResult = await _controller.GetAllDoctors(1, 1);

            // Assert
            Assert.IsNotNull(actionResult, "ActionResult should not be null.");

            if (actionResult is OkObjectResult okResult)
            {
                Assert.IsInstanceOfType(okResult, typeof(OkObjectResult), "Result should be of type OkObjectResult");
                var response = okResult.Value;
                Assert.IsNotNull(response, "Response should not be null.");

                // Convert the response to a dictionary for easier property access
                var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(response));

                Console.WriteLine("Response properties:");
                foreach (var kvp in responseDict)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }

                Assert.IsTrue(responseDict.ContainsKey("TotalDoctors"), "Response should contain 'TotalDoctors'");
                Assert.AreEqual(doctors.Count, Convert.ToInt32(responseDict["TotalDoctors"]), "TotalDoctors count does not match.");

                Assert.IsTrue(responseDict.ContainsKey("PageNumber"), "Response should contain 'PageNumber'");
                Assert.AreEqual(1, Convert.ToInt32(responseDict["PageNumber"]), "PageNumber does not match.");

                Assert.IsTrue(responseDict.ContainsKey("PageSize"), "Response should contain 'PageSize'");
                Assert.AreEqual(1, Convert.ToInt32(responseDict["PageSize"]), "PageSize does not match.");

                Assert.IsTrue(responseDict.ContainsKey("Doctors"), "Response should contain 'Doctors'");
                var returnedDoctors = JsonConvert.DeserializeObject<List<DoctorDto>>(JsonConvert.SerializeObject(responseDict["Doctors"]));
                Assert.IsNotNull(returnedDoctors, "Doctors list should not be null.");
                Assert.AreEqual(doctors.Count, returnedDoctors.Count, "Number of returned doctors does not match.");

                for (int i = 0; i < doctors.Count; i++)
                {
                    Assert.AreEqual(doctors[i].FirstName, returnedDoctors[i].FirstName, $"FirstName of doctor at index {i} does not match.");
                    Assert.AreEqual(doctors[i].LastName, returnedDoctors[i].LastName, $"LastName of doctor at index {i} does not match.");
                    Assert.AreEqual(doctors[i].Specialization, returnedDoctors[i].Specialization, $"Specialization of doctor at index {i} does not match.");
                    Assert.AreEqual(doctors[i].City, returnedDoctors[i].City, $"City of doctor at index {i} does not match.");
                }
            }
            else if (actionResult is NotFoundObjectResult notFoundResult)
            {
                Assert.Fail("Expected OkObjectResult but got NotFoundObjectResult");
            }
            else
            {
                Assert.Fail($"Unexpected result type: {actionResult.GetType().Name}");
            }
        }

        [TestMethod]
        public async Task TC002_VerifyHandlingWhenNoDoctorsAvailable()
        {
            // Arrange
            _doctorRepoMock.Setup(repo => repo.GetAllDoctorsAsync(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new List<DoctorDto>());
            _doctorRepoMock.Setup(repo => repo.GetTotalNumberOfDoctorsAsync())
                           .ReturnsAsync(0);

            // Act
            var result = await _controller.GetAllDoctors(1, 20) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult), "Result should be of type NotFoundObjectResult");

            Assert.IsNotNull(result.Value, "Result.Value should not be null.");

            // Convert the result to a string for inspection
            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result.Value);
            Console.WriteLine($"Result Value: {resultJson}");

            // Parse the JSON string back to a dynamic object
            dynamic response = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJson);

            // Check if the 'Message' property exists (note the capital 'M')
            Assert.IsTrue(((Newtonsoft.Json.Linq.JObject)response).ContainsKey("Message"), "Response should contain a 'Message' property.");
            Assert.AreEqual("No doctors found", response.Message.ToString(), "The response message should indicate no doctors are found.");
        }

        // Add this class if it doesn't exist
        public class ErrorResponse
        {
            public string Message { get; set; }
        }
    }
}
