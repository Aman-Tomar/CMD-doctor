using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace CMD.Domain.Services
{
    /// <summary>
    /// Provides a service to retrieve custom exception messages from an XML file.
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly Dictionary<string, string> _messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// Loads custom messages from an XML file.
        /// </summary>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/> used for app settings.</param>
        /// <exception cref="FileNotFoundException">
        /// Thrown if the XML file containing messages is not found.
        /// </exception>
        public MessageService(IConfiguration configuration)
        {
            // Load messages from XML file
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var xmlFilePath = Path.Combine(assemblyLocation, "Resources", "DoctorCustomExceptionMessage.xml");
            _messages = LoadMessages(xmlFilePath);
        }

        /// <summary>
        /// Loads exception messages from an XML file into a dictionary.
        /// </summary>
        /// <param name="xmlFilePath">The path to the XML file containing messages.</param>
        /// <returns>A dictionary with message keys and their corresponding messages.</returns>
        /// <exception cref="FileNotFoundException">
        /// Thrown if the XML file is not found.
        /// </exception>
        private Dictionary<string, string> LoadMessages(string xmlFilePath)
        {
            var messages = new Dictionary<string, string>();
            if (File.Exists(xmlFilePath))
            {
                var document = XDocument.Load(xmlFilePath);
                messages = document.Descendants("Message")
                                   .ToDictionary(
                                       m => m.Attribute("key")?.Value,
                                       m => m.Value
                                   );
            }
            else
            {
                // Handle the case where the file doesn't exist
                throw new FileNotFoundException("Exception messages file not found.", xmlFilePath);
            }

            return messages;
        }

        /// <summary>
        /// Retrieves a message based on the provided key.
        /// </summary>
        /// <param name="key">The key associated with the message.</param>
        /// <returns>The message if found; otherwise, a default message "Unknown error occurred".</returns>
        public string GetMessage(string key)
        {
            return _messages.TryGetValue(key, out var message) ? message : "Unknown error occurred.";
        }
    }
}
