﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMD.Domain.Services;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Moq;
using Microsoft.Extensions.Configuration;

namespace CMD.Test
{
    /// <summary>
    /// Unit tests for the <see cref="MessageService"/> class.
    /// </summary>
    [TestClass]
    public class MessageServiceTests
    {
        private Mock<IConfiguration> _configurationMock;
        private string _xmlFilePath;

        /// <summary>
        /// Initializes the test environment by setting up mocks and creating a mock XML file.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();

            // Set up path to the mock XML file
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _xmlFilePath = Path.Combine(assemblyLocation, "Resources", "DoctorCustomExceptionMessage.xml");

            // Create a mock XML file for testing purposes
            if (!Directory.Exists(Path.Combine(assemblyLocation, "Resources")))
            {
                Directory.CreateDirectory(Path.Combine(assemblyLocation, "Resources"));
            }

            var xmlContent = @"<Messages>
                                <Message key='ERR001'>Invalid doctor ID.</Message>
                                <Message key='ERR002'>Doctor not found.</Message>
                               </Messages>";

            File.WriteAllText(_xmlFilePath, xmlContent);
        }

        /// <summary>
        /// Cleans up the test environment by deleting the mock XML file after tests.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            // Delete the mock XML file after tests
            if (File.Exists(_xmlFilePath))
            {
                File.Delete(_xmlFilePath);
            }
        }

        /// <summary>
        /// Tests that the <see cref="MessageService"/> correctly loads a message from the XML file.
        /// </summary>
        [TestMethod]
        public void MessageService_ValidFile_LoadsMessages()
        {
            // Arrange
            var messageService = new MessageService(_configurationMock.Object);

            // Act
            var result = messageService.GetMessage("ERR001");

            // Assert
            Assert.AreEqual("Invalid doctor ID.", result);
        }

        /// <summary>
        /// Tests that the <see cref="MessageService"/> returns a default message when an invalid key is provided.
        /// </summary>
        [TestMethod]
        public void MessageService_InvalidKey_ReturnsDefaultMessage()
        {
            // Arrange
            var messageService = new MessageService(_configurationMock.Object);

            // Act
            var result = messageService.GetMessage("INVALID_KEY");

            // Assert
            Assert.AreEqual("Unknown error occurred.", result);
        }

        /// <summary>
        /// Tests that the <see cref="MessageService"/> correctly loads messages from the XML file.
        /// </summary>
        [TestMethod]
        public void LoadMessages_FileExists_ReturnsCorrectMessages()
        {
            // Arrange
            var messageService = new MessageService(_configurationMock.Object);

            // Act
            var messages = typeof(MessageService)
                .GetMethod("LoadMessages", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(messageService, new object[] { _xmlFilePath }) as Dictionary<string, string>;

            // Assert
            Assert.IsNotNull(messages);
            Assert.IsTrue(messages.ContainsKey("ERR001"));
            Assert.AreEqual("Invalid doctor ID.", messages["ERR001"]);
        }
    }
}
