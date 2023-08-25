using System.Collections.Immutable;
using MongoDB.Bson;
using MongoDBBenchmark.Placeholder;

namespace MongoDBBenchmark.Generator;

public record DocumentTemplate
{
    public string RawTemplate { init; get;}
    public BsonDocument Document { init; get;}
    public ImmutableDictionary<string, IPlaceholder> Placeholders { init; get;}
    
    public DocumentTemplate(string rawTemplate, BsonDocument document, ImmutableDictionary<string, IPlaceholder> placeholders)
    {
        this.RawTemplate = rawTemplate;
        Document = document;
        Placeholders = placeholders;
    }
    
    public static DocumentTemplate FromString(string rawTemplate)
    {
        return FromDocument(BsonDocument.Parse(rawTemplate));
    }
    
    public static DocumentTemplate FromDocument(BsonDocument document)
    {
        return new DocumentTemplate(document.ToString(), document, MapPlaceholders(document).ToImmutableDictionary());
    }
    
    /**
     * Stores which fields are placeholders. To apply them later.
     */
    private static Dictionary<string, IPlaceholder> MapPlaceholders(BsonDocument document)
    {
        Dictionary<string, IPlaceholder> placeholders = new();
        document.Names
            .Where(documentName => document.GetValue(documentName).IsString).ToList()
            .ForEach(documentName =>
            {
                var val = document.GetValue(documentName);
                var placeholder = IPlaceholder.TryParse(val.AsString);
                if (placeholder != null)
                    placeholders.Add(documentName, placeholder);
            });
        return placeholders;
    }
    
}