using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface IRedisService
    {
        Task SetTokenAsync(string key, string value, TimeSpan expiration);
        Task<string?> GetTokenAsync(string key);
    }
}