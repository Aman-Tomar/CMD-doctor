using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Domain.Services
{
    /// <summary>
    /// Defines a contract for a service that retrieves messages based on a key.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Retrieves a message corresponding to the provided key.
        /// </summary>
        /// <param name="key">The key associated with the desired message.</param>
        /// <returns>The message if found; otherwise, a default error message.</returns>
        string GetMessage(string key);
    }
}
