using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(User user, string userRole);
        string CreateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}