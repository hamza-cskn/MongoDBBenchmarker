using System.Collections.Immutable;
using MongoDB.Bson;

namespace MongoDBBenchmark.Generator;

public record DocumentTemplate
{
    public string RawTemplate { init; get;}
    public BsonDocument Document { init; get;}
    public ImmutableDictionary<string, Placeholder> Placeholders { init; get;}
    
    public DocumentTemplate(string rawTemplate, BsonDocument document, ImmutableDictionary<string, Placeholder> placeholders)
    {
        this.RawTemplate = rawTemplate;
        Document = document;
        Placeholders = placeholders;
    }
    
}