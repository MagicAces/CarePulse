using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Helpers;

namespace backend.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(SendEmailObject emailObject);
    }
}