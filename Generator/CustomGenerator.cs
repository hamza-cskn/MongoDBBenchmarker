using System.Collections.Immutable;
using MongoDB.Bson;

namespace MongoDBBenchmark.Generator;

public class CustomGenerator : IDocumentGenerator
{
    private BsonDocument _document;
    private ImmutableDictionary<string, Placeholder> _placeholders;
    
    public CustomGenerator(BsonDocument document, ImmutableDictionary<string, Placeholder> placeholders)
    {
        _document = document;
        _placeholders = placeholders.ToImmutableDictionary();
    }

    public BsonDocument[] Generate(int amount)
    {
        var documents = new BsonDocument[amount];
        for (var i = 0; i < amount; i++)
            documents[i] = GenerateOne(i);
        return documents;
    }

    private BsonDocument GenerateOne(int id)
    {
        var document = (BsonDocument) _document.Clone();
        foreach (var (key, value) in _placeholders)
            value.Apply(id, key, document);
        return document;
    }
    
}