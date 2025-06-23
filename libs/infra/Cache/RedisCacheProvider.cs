using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.Infra.Cache
{
    public class RedisCacheProvider : ICacheProvider
    {
        private ConnectionMultiplexer? _redisConnection;

        private IConfiguration _config;

        private readonly ILogger<RedisCacheProvider> _logger;

        public RedisCacheProvider(IConfiguration config, ILogger<RedisCacheProvider> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            
        }

        public async Task RedisConnectAsync()
        {
            if (_redisConnection == null)
            {
                string redis_host=_config["REDIS_HOST"] ?? string.Empty;
                var isPortInt = int.TryParse(_config["REDIS_PORT"], out int redis_port);
                string? redisPassword = _config["REDIS_PASSWORD"] ?? string.Empty;

                if(!string.IsNullOrWhiteSpace(redis_host)
                    &&!string.IsNullOrWhiteSpace(redisPassword)
                    && isPortInt)
                {
                    try
                    {
                        var connredString= $"{redis_host}:{redis_port},password={redisPassword},name=api,connectTimeout=5000,abortConnect=false,allowAdmin=true,defaultDatabase=0";
                        _logger.LogDebug($"Connecting to Redis with connection string: {connredString}");
                        _redisConnection = await ConnectionMultiplexer.ConnectAsync(connredString);
                        _logger.LogInformation("Redis connection established successfully.");
                    }
                    catch (System.Exception ex)
                    {
                        _logger.LogError($"Error while initializing Redis connection.{ex.Message}");                       
                    }
                }
                else
                {
                    _logger.LogError("Redis connection string is not properly configured. Please check REDIS_HOST, REDIS_PORT, and REDIS_PASSWORD in your configuration settings.");
                }                
            }
            if(_redisConnection == null || !_redisConnection.IsConnected)
            {
                _logger.LogError("RedisCacheProvider:No Connection, Caching disabled.");
                throw new System.Exception("RedisCacheProvider:No Connection, Caching disabled.");
            }
        }

        public async Task<IDatabaseAsync> GetRedisDBAsync()
        {
            await RedisConnectAsync();
            var resdisDb = _redisConnection!.GetDatabase();
            return resdisDb;
        }
        public async Task<T> GetAsync<T>(string key)
        {
           var redisDb= await GetRedisDBAsync();
            var value = await redisDb.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return default(T)!;
            }
            if(typeof(T) == typeof(string))
            {
                return (T)(object)value.ToString()!;
            }

            var tVal=JsonConvert.DeserializeObject<T>(value!);
            if(tVal == null)
                return default(T)!;

            return tVal;           
        }

        //public async Task<List<string>?> GetAllKeyAsync<T>(string key) => await GetAllKeyPatternAsync("*", 10000);

        //private async Task<List<string>> GetAllKeyPatternAsync<T>(string pattern, int pageSize=10000)
        //{
        //    var result= new List<string>();
        //    var redisDb = await GetRedisDBAsync();

        //    foreach(var redisServer in _redisConnection.GetServers())
        //    {
        //        await Task.Run(() =>
        //        {
        //            foreach (var key in redisServer.Keys(pattern: pattern, pageSize: pageSize))
        //            {
        //                result.Add(key.ToString()!);
        //            }
        //        });               
        //    }
        //    return result;
        //}

        public async Task<bool> RemoveAsync(string key)
        {
            var redisDb = await GetRedisDBAsync();

            if(await redisDb.KeyExistsAsync(key))
            {
                var result = await redisDb.KeyDeleteAsync(key);
                if (result)
                {
                    _logger.LogDebug($"Removed cache for key: {key}.");
                }
                return result;
            }
            else
            {
                _logger.LogDebug($"Key: {key} does not exist in cache.");
                return false;
            }
        }

        public async Task SetAsync<T>(string key, T value)
        {
            await this.SetAsync(key, value, TimeSpan.Zero);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan timeout)
        {
            if(value == null)
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");

            var redisDb = await GetRedisDBAsync();
            if(value.GetType() == typeof(string))
            {
                await redisDb.StringSetAsync(key,(string)(object) value, timeout, When.Always);
            }
            else
            {
                var jsonValue = JsonConvert.SerializeObject(value);
                await redisDb.StringSetAsync(key, jsonValue, timeout,When.Always);
            }
        }

        public Task<List<string>> GetListAsync<T>(string key)
        {
            throw new NotImplementedException();
        }
    }
}
