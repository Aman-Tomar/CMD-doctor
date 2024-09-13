using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Domain.Services
{
    public interface IMessageService
    {
        string GetMessage(string key);
    }
}
