using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using StackExchange.Redis;

namespace backend.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _connection;

        public RedisService(IConnectionMultiplexer connection)
        {
            _connection = connection;
        }

        public async Task<string?> GetTokenAsync(string key)
        {
            var db = _connection.GetDatabase();
            var transaction = db.CreateTransaction();

            var tokenTask = transaction.StringGetAsync(key);
            var deleteTask = transaction.KeyDeleteAsync(key);
            await transaction.ExecuteAsync();

            // Retrieve the token
            var token = await tokenTask;
            await deleteTask;
            return token;
        }

        public async Task SetTokenAsync(string key, string value, TimeSpan expiration)
        {
            var db = _connection.GetDatabase();
            await db.StringSetAsync(key, value, expiration);
        }
    }
}