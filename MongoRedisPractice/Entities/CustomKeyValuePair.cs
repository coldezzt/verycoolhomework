using MongoDB.Bson.Serialization.Attributes;

namespace MongoRedisPractice.Entities;

public class CustomKeyValuePair
{
    [BsonId]
    public string Key { get; set; }
    public string Value { get; set; }
}