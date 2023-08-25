using MongoDB.Bson;

namespace MongoDBBenchmark.Placeholder;

public interface IPlaceholder
{
    static IPlaceholder? TryParse(string str)
    {
        return new IPlaceholder?[]
        {
            StringPlaceholder.FromString(str),
            IntPlaceholder.FromString(str),
            IdPlaceholder.FromString(str)
        }.FirstOrDefault(x => x != null);
    }
    void Apply(int id, String key, BsonDocument bsonDocument);
}