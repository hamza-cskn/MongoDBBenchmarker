using System.Collections.Immutable;
using MongoDB.Bson;
using MongoDBBenchmark.Generator;

namespace MongoDBBenchmark;

public class InputManager
{
    public static Func<string, string> GetStringInput => (msg) =>
        GetInput(msg, s => s);
    
    public static Func<string, int> GetIntInput = (msg) =>
        GetInput(msg, int.Parse, i => i > 0);

    public static Func<string, DocumentTemplate> GetDocumentTemplateInput = (msg) =>
        GetInput(msg, (str) =>
        {
            var document = BsonDocument.Parse(str);
            return new DocumentTemplate(str, document, MapPlaceholders(document).ToImmutableDictionary());
        });
    
    public static Func<string, BsonDocument> GetDocumentInput = (msg) =>
        GetInput(msg, BsonDocument.Parse);

    public static Func<BsonDocument> GetFilterInput = () =>
        GetDocumentInput("Please enter document for filtering:");
    
    public static Func<Operations> GetOperationInput = () =>
        GetInput("Supported Operations: Insert, Update, Delete, Read\nPlease enter operation type: ", 
            s => OperationsExtensions.FromString(s)!.Value);

    /**
     * Returns document generator based on user input.
     */
    public static IDocumentGenerator GetDocumentGeneratorInput()
    {
        Console.WriteLine("Please enter document type. ");
        Console.WriteLine("You can use templates: User, Product, Order");
        Console.WriteLine("Or you can write your own document in BSON format.");
        Console.WriteLine("Custom document example: {\"name\": \"Hamza\", \"age\": 19}");
        Console.WriteLine("You can use these placeholders in documents as string:");
        Console.WriteLine("- '%int,1,10%' - random integer between 1 and 10");
        Console.WriteLine("- '%id%' - iteration number");
        Console.WriteLine("- '%string(5)%' - random string with length 5");
        var document = GetDocumentTemplateInput("Please enter template document for generating:");
        return new CustomGenerator(document);
    }

    /**
     * Stores which fields are placeholders. To apply them later.
     */
    private static Dictionary<string, Placeholder> MapPlaceholders(BsonDocument document)
    {
        Dictionary<string, Placeholder> placeholders = new();
        document.Names
            .Where(documentName => document.GetValue(documentName).IsString).ToList()
            .ForEach(documentName =>
                {
                    var val = document.GetValue(documentName);
                    var placeholder = Placeholder.TryParse(val.AsString);
                    if (placeholder != null)
                        placeholders.Add(documentName, placeholder);
                });
        return placeholders;
    }

    /**
     *  Generic input method. Gets input from user, parses and validates it.
     *  If input is invalid, it will ask again.
     *  If input is null, assumes it is invalid.
     *  If input is "exit", it will exit the program.
     */
    public static T GetInput<T>(string message, Func<string, T> parser, Func<T, bool>? validate = null)
    {
        Console.WriteLine(message);
        string? input = Console.ReadLine();
        if (input == "exit")
            Environment.Exit(0);
        if (input == null)
            return GetInput(message, parser, validate);
        
        T val;
        try
        {
            val = parser(input);
        } catch (Exception e)
        {
            Console.WriteLine("Error! Input could not parsed.");
            Console.WriteLine(e.Message);
            return GetInput(message, parser, validate);
        }
        
        if (validate != null && !validate(val))
        {
            Console.WriteLine("Error! Invalid input.");
            return GetInput(message, parser, validate);
        }
        return val;
    }
}