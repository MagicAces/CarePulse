using System.Text;
using backend.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services
{
    public class SecretService : ISecretService
    {
        private readonly IConfiguration _config;

        public SecretService(IConfiguration config)
        {
            _config = config;
        }

        public SymmetricSecurityKey GenerateKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        }

        public bool ValidateSecret(string secret)
        {
            var storedSecret = _config["AdminSettings:SecretKey"];
            return storedSecret == secret;
        }
    }
}