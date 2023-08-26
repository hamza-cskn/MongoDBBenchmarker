// See https://aka.ms/new-console-template for more information

// get input

using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDBBenchmark;
using MongoDBBenchmark.Generator;
using static MongoDBBenchmark.InputManager;

class Program {
    private const string ExitInput = "exit";
    private const string BackInput = "back";
    private Client _client = new Client();

    static void Main(string[] args)
    {
        if (args.Length == 1 && args[0] == "--config")
        {
            Console.WriteLine("Config mode on. No input will receive from user.");
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("operations.json")
                .Build();
            new Program().RunUsingConfig(config);
            return;
        }
        new Program().RunUsingCli();
    }

    void RunUsingConfig(IConfiguration config)
    {
        _client.Connect(Environment.GetEnvironmentVariable("BENCHMARK_CONNECTION_STRING"),
            Environment.GetEnvironmentVariable("BENCHMARK_DATABASE_NAME"),
            Environment.GetEnvironmentVariable("BENCHMARK_COLLECTION_NAME"));
        BsonSerializer.RegisterSerializer(new ObjectSerializer(type => true));
        foreach (var query in config.GetSection("Operations").GetChildren())
        {
            Console.WriteLine("Operation started: " + query.Key);
            var operation = OperationsExtensions.FromString(query["OperationType"]);
            if (operation == null)
            {
                Console.WriteLine("Invalid operation type: " + query["OperationType"]);
                continue;
            }
            switch (operation)
            {
                case Operations.Insert:
                    _client.Insert(Int32.Parse(query["Amount"]), 
                        new CustomGenerator(DocumentTemplate.FromDocument(Section2BsonDocument(query.GetSection("Template")))));
                    break;
                case Operations.Read:
                    _client.Read(Section2BsonDocument(query.GetSection("Filter")));
                    break;
                case Operations.Update:
                    _client.Update(Section2BsonDocument(query.GetSection("Filter")), 
                        Section2BsonDocument(query.GetSection("Update")));
                    break;
                case Operations.Delete:
                    _client.Delete(Section2BsonDocument(query.GetSection("Filter")));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("invalid operation type: " + operation);
            }
        }
    }
    
    BsonDocument Section2BsonDocument(IConfigurationSection section)
    {
        BsonDocument bsonDocument = new BsonDocument();
        foreach (var child in section.GetChildren())
        {
             if (child.Value != null) bsonDocument.Add(new BsonElement(child.Key, child.Value));
             else bsonDocument.Add(new BsonElement(child.Key, Section2BsonDocument(child)));
        }

        return bsonDocument;
    }

    void RunUsingCli()
    {
        Console.WriteLine("Welcome to Mongo DB Benchmark.");
        Console.WriteLine("Write 'exit' to exit.");
        _client.Connect(GetStringInput("Please enter connection string: "),
            GetStringInput("Please enter database name: "),
            GetStringInput("Please enter collection name: "));
        while (true)
            StartNewOperation();
    }

    void StartNewOperation()
    {
        Operations operation = GetOperationInput();
        switch (operation)
        {
            case Operations.Insert:
                InsertOperation(GetDocumentGeneratorInput());
                break;
            case Operations.Read:
                ReadOperation();
                break;
            case Operations.Update:
                _client.Update(GetFilterInput(), GetDocumentInput("Please enter update document: "));
                break;
            case Operations.Delete:
                _client.Delete(GetFilterInput());
                break;
            default:
                throw new ArgumentOutOfRangeException("invalid operation type: " + operation);
        }
    }

    void ReadOperation()
    {
        BsonDocument filter = GetFilterInput();
        List<BsonDocument> result = _client.Read(filter);
        Console.WriteLine("Write 'back' to go back to main menu or 'exit' to exit.");
        if (result.Count > 0)
        {
            Console.WriteLine("Write 'show(n)' to show first n documents.");
            ShowResults(result);
        }
    }

    void InsertOperation(IDocumentGenerator documentGenerator)
    {
        int numOfDocuments = GetIntInput("Please enter number of documents to generate: ");
        Console.WriteLine("Starting benchmark.");
        _client.Insert(numOfDocuments, documentGenerator);
        Console.WriteLine("Write 'back' to go back to main menu or 'exit' to exit.");
        Console.WriteLine("Write anything else to insert again.");
        var input = Console.ReadLine();
        if (input == ExitInput)
            Environment.Exit(0);
        if (input == BackInput)
            return;
        InsertOperation(documentGenerator);
    }

    void ShowResults(List<BsonDocument> result)
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (input == ExitInput)
                Environment.Exit(0);
            if (input == BackInput)
                return;
            if (input == null)
                continue;
            if (input.StartsWith("show(") && input.EndsWith(")"))
            {
                int n;
                if (!Int32.TryParse(input.Substring(5, input.Length - 6), out n))
                {
                    Console.WriteLine("Error! Invalid number.");
                    continue;
                }
                for (int i = 0; i < Math.Min(n, result.Count); i++)
                {
                    Console.WriteLine(i + ":" + result[i]);
                }
            }
        }
    }
}