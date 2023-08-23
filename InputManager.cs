using MongoDB.Bson;

namespace MongoDBBenchmark;

public class InputManager
{
    public static string GetConnectionString()
    {
        Console.WriteLine("Please enter your MongoDB connection string: ");
        var input = Console.ReadLine();
        if (input == "exit")
            Environment.Exit(0);
        if (input == null)
            return GetConnectionString();
        return input;
    }
    
    public static int GetNumberOfDocuments()
    {
        Console.WriteLine("Please enter number of documents: ");
        var input = Console.ReadLine();
        if (input == "exit")
            Environment.Exit(0);
        if (!Int32.TryParse(input, out int numOfDocuments))
        {
            Console.WriteLine("Error! Invalid number.");
            return GetNumberOfDocuments();
        }
        return numOfDocuments;
    }

    public static Func<int, BsonDocument> GetDocumentGenerator()
    {
        Console.WriteLine("Please enter document type. ");
        Console.WriteLine("You can use templates: User, Product, Order");
        Console.WriteLine("Or you can write your own document in BSON format.");
        Console.WriteLine("Custom document example: {\"name\": \"John\", \"age\": 20}");
        Console.WriteLine("You can use these placeholders in custom documents:");
        Console.WriteLine("'{int,1,10}' - random integer between 1 and 10");
        Console.WriteLine("'%id%' - iteration number");
        Console.WriteLine("'%string(5)%' - random string with length 5");
        Console.WriteLine("Please enter document: ");
        var input = Console.ReadLine();
        if (input == "exit")
            Environment.Exit(0);
        var document = BsonDocument.Parse(input); // todo check parse errors
        if (document == null)
        {
            Console.WriteLine("Error! Invalid document type.");
            return GetDocumentGenerator();
        }

        Dictionary<string, Placeholder> placeholders = new();
        foreach (var documentName in document.Names)
        {
            var val = document.GetValue(documentName);
            if (!val.IsString)
                continue;
            var placeholder = Placeholder.TryParse(val.AsString);
            if (placeholder == null)
                continue;
            placeholders.Add(documentName, placeholder);
        }
        return (id) =>
        {
            document = (BsonDocument)document.Clone();
            foreach (var (key, value) in placeholders)
            {
                value.Apply(id, key, document);
            }
            return document;
        };
    }
    
    public static Operations GetOperation()
    {
        Console.WriteLine("Supported Operations: Insert, Update, Delete, Read");
        Console.WriteLine("Please enter operation type: ");
        var input = Console.ReadLine();
        if (input == "exit")
            Environment.Exit(0);
        var operation = OperationsExtensions.FromString(input);
        if (operation == null)
        {
            Console.WriteLine("Error! Invalid operation type.");
            return GetOperation();
        }
        return operation.Value;
    }
    
    public static BsonDocument GetFilter()
    {
        Console.WriteLine("Please enter filter: ");
        var input = Console.ReadLine();
        if (input == "exit")
            Environment.Exit(0);
        var filter = BsonDocument.Parse(input); // todo check parse errors
        if (filter == null)
        {
            Console.WriteLine("Error! Invalid filter.");
            return GetFilter();
        }
        return filter;
    }

    public static string GetDatabaseName()
    {
        Console.WriteLine("Please enter database name: ");
        var input = Console.ReadLine();
        if (input == "exit")
            Environment.Exit(0);
        if (input == null)
        {
            return GetDatabaseName();
        }
        return input;
    }
    
    public static string GetCollectionName()
    {
        Console.WriteLine("Please enter collection name: ");
        var input = Console.ReadLine();
        if (input == "exit")
            Environment.Exit(0);
        if (input == null)
        {
            return GetCollectionName();
        }
        return input;
    }
}