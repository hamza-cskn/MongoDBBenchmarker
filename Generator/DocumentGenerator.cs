using MongoDB.Bson;

namespace MongoDBBenchmark.Generator;

public interface IDocumentGenerator
{
    BsonDocument[] Generate(int amount);

    string GetRawTemplate();
}