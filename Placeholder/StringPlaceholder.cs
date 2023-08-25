using MongoDB.Bson;
using MongoDBBenchmark.Placeholder;

namespace MongoDBBenchmark;

public class StringPlaceholder : IPlaceholder
{
    private int _length;

    public static StringPlaceholder? FromString(string str)
    {
        if (!str.StartsWith("%string(") || !str.EndsWith(")%"))
        {
            return null;
        }
        var length = str.Substring(8, str.Length - 10);
        if (!int.TryParse(length, out var lengthInt))
        {
            return null;
        }
        return new StringPlaceholder(lengthInt);
    }
    
    public StringPlaceholder(int length)
    {
        this._length = length;
    }
    
    public void Apply(int id, String key, BsonDocument bsonDocument)
    {
        bsonDocument.Set(key, GenerateString(_length));
    }
    
    private static String GenerateString(int length)
    {
        string chars = "abcdefghklmopqrstuvwxyABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
}