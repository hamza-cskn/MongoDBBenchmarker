using MongoDB.Bson;

namespace MongoDBBenchmark;

public class IdPlaceholder : Placeholder
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