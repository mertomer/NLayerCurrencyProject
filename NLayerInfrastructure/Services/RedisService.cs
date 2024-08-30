using StackExchange.Redis;
using System.Threading.Tasks;

namespace NLayerInfrastructure.Services
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisService(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public async Task SetExchangeRateAsync(string key, string value)
        {
            await _db.StringSetAsync(key, value);
        }

        public async Task DeleteExchangeRateAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }


        public async Task<string> GetExchangeRateAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }
    }
}
