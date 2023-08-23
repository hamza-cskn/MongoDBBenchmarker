using MongoDB.Bson;

namespace MongoDBBenchmark;

public class IntPlaceholder : Placeholder
{
    private int _min;
    private int _max;
    
    public IntPlaceholder(int min, int max)
    {
        this._min = min;
        this._max = max;
    }
    
    public static IntPlaceholder? FromString(string str)
    {
        if (!str.StartsWith("%int(") || !str.EndsWith(")%"))
        {
            return null;
        }

        var parameters = str.Substring(5, str.Length - 7).Split(',');
        var min = parameters[0];
        var max = parameters[1];
        if (!int.TryParse(min, out var minInt) || !int.TryParse(max, out var maxInt))
        {
            return null;
        }
        return new IntPlaceholder(minInt, maxInt);
    }
    
    public void Apply(int id, String key, BsonDocument bsonDocument)
    {
        bsonDocument.Set(key, new Random().Next(_min, _max));
    }
}