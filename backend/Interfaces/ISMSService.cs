using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface ISMSService
    {
        Task<bool> SendSmsAsync(string to, string message);
    }
}