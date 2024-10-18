using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using backend.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace backend.Providers
{
    public class CustomOtpTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        private readonly IRedisService _redisService;
        private readonly TimeSpan _expirationTime;

        public CustomOtpTokenProvider(IDataProtectionProvider dataProtectionProvider,
                                      IOptions<DataProtectionTokenProviderOptions> options,
                                      ILogger<DataProtectorTokenProvider<TUser>> logger,
                                      IRedisService redisService)
            : base(dataProtectionProvider, options, logger)
        {
            _redisService = redisService;
            _expirationTime = options.Value.TokenLifespan;
        }

        public override async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            using var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            int code = BitConverter.ToInt32(bytes, 0) % 1000000;
            code = Math.Abs(code);
            string token = code.ToString("D6");

            var secureToken = await base.GenerateAsync(purpose, manager, user);

            await _redisService.SetTokenAsync($"{token}_{purpose}", secureToken, _expirationTime);
            return token;
        }

        public override async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            if (token.Length != 6 || !int.TryParse(token, out _))
                return false;

            string key = $"{token}_{purpose}";
            var secureToken = await _redisService.GetTokenAsync(key);
            if (secureToken == null)
                return false;

            return await base.ValidateAsync(purpose, secureToken, manager, user);
        }
    }
}