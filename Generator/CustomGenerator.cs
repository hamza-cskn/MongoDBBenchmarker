using System.Collections.Immutable;
using MongoDB.Bson;

namespace MongoDBBenchmark.Generator;

public class CustomGenerator : IDocumentGenerator
{
    private readonly DocumentTemplate _template;
    
    public CustomGenerator(DocumentTemplate template)
    {
        _template = template;
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
        var document = (BsonDocument) _template.Document.Clone();
        foreach (var (key, value) in _template.Placeholders)
            value.Apply(id, key, document);
        return document;
    }
    
    public string GetRawTemplate()
    {
        return _template.RawTemplate;
    }

    
}