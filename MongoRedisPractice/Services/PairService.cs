using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using MongoRedisPractice.Entities;
using StackExchange.Redis;

namespace MongoRedisPractice.Services;

public class PairService
{
    private readonly IDatabase _redis;
    private readonly PairRepository _pairRepo;

    public PairService(HttpClient client,
        IConnectionMultiplexer muxer,
        PairRepository pairRepo)
    {
        _pairRepo = pairRepo;
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("testApp","1.0") );
        _redis = muxer.GetDatabase();
    }

    public async Task<string?> GetAsync(string value)
    {
        var redisValue = _redis.StringGet(value);

        if (!redisValue.IsNullOrEmpty)
            return redisValue;

        // searching in db
        var dbPair = await _pairRepo.GetAsync(value);
        if (dbPair is null) return null;
        
        await _redis.StringSetAsync(value, dbPair.Key);
        return dbPair.Key;
    }
    
    public async Task<string?> PostAsync(string value)
    {
        var redisValue = _redis.StringGet(value);

        if (!redisValue.IsNullOrEmpty)
            return redisValue;
        
        var id = Guid.NewGuid().ToString();
        await _pairRepo.AddAsync(new CustomKeyValuePair {Key = id, Value = value});
        await _redis.StringSetAsync(value, id);
        return id;
    }
    
    public async Task<bool> DeleteAsync(string value)
    {
        var redisValue = _redis.StringGet(value);
        if (redisValue.IsNullOrEmpty)
            return true;

        await _redis.KeyDeleteAsync(value);
        await _pairRepo.DeleteAsync(redisValue!);
        return true;
    }
}