using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoRedisPractice.Entities;

namespace MongoRedisPractice.Services;

public class PairRepository
{
    private readonly IMongoCollection<CustomKeyValuePair> _customCollection;
    
    public PairRepository(IOptions<TestDatabaseSettings> testDatabaseSettings)
    {
        var mongoClient = new MongoClient(testDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(testDatabaseSettings.Value.DatabaseName);
        _customCollection = mongoDatabase.GetCollection<CustomKeyValuePair>(testDatabaseSettings.Value.CollectionName);
    }
    
    // get
    public async Task<CustomKeyValuePair?> GetAsync(string key) =>
        await _customCollection.Find(c => c.Key == key).FirstOrDefaultAsync();
    
    // post
    public async Task AddAsync(CustomKeyValuePair ckvp) =>
        await _customCollection.InsertOneAsync(ckvp);
    
    // delete
    public async Task DeleteAsync(string key) =>
        await _customCollection.DeleteOneAsync(c => c.Key == key);
}