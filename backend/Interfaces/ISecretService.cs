using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace backend.Interfaces
{
    public interface ISecretService
    {
        bool ValidateSecret(string secret);
        SymmetricSecurityKey GenerateKey();
    }
}