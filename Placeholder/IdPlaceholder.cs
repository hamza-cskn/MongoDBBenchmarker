using MongoDB.Bson;
using MongoDBBenchmark.Placeholder;

namespace MongoDBBenchmark;

public class IdPlaceholder : IPlaceholder
{
    public static IdPlaceholder? FromString(string str)
    {
        if (!str.Equals("%id%"))
        {
            return null;
        }
        return new IdPlaceholder();
    }
    
    public void Apply(int id, String key, BsonDocument bsonDocument)
    {
        bsonDocument.Set(key, id);
    }   
}