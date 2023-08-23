using System.Collections.Immutable;
using MongoDB.Bson;

namespace MongoDBBenchmark;

public interface Placeholder
{
    static Placeholder? TryParse(string str)
    {
        return new Placeholder?[]
        {
            StringPlaceholder.FromString(str),
            IntPlaceholder.FromString(str),
            IdPlaceholder.FromString(str)
        }.FirstOrDefault(x => x != null);
    }
    void Apply(int id, String key, BsonDocument bsonDocument);
}