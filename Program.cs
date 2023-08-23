// See https://aka.ms/new-console-template for more information

// get input

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBBenchmark;
using MongoDBBenchmark.Generator;
using static MongoDBBenchmark.InputManager;

const string exitInput = "exit";
const string backInput = "back";
IMongoCollection<BsonDocument> collection;

Console.WriteLine("Welcome to Mongo DB Benchmark.");
Console.WriteLine("Write 'exit' to exit.");
ConnectToDatabase();
while (true)
    StartNewOperation();

void ConnectToDatabase()
{
    MongoClient client;
    var connectionString = GetStringInput("Please enter connection string: ");
    try {
        client = new MongoClient(connectionString);
    } catch (Exception e) {
        Console.WriteLine("Could not connected to the database.");
        Console.WriteLine(e.Message);
        ConnectToDatabase();
        return;
    }
    var databaseName = GetStringInput("Please enter database name: ");
    var collectionName = GetStringInput("Please enter collection name: ");
    collection = client
        .GetDatabase(databaseName)
        .GetCollection<BsonDocument>(collectionName);
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
            UpdateOperation();
            break;
        case Operations.Delete:
            DeleteOperation();
            break;
        default:
            throw new ArgumentOutOfRangeException("invalid operation type: " + operation);
    }
}

void ReadOperation()
{
    BsonDocument filter = GetFilterInput();
    Console.WriteLine("Starting benchmark.");
    var stopwatch = new Stopwatch();
    stopwatch.Start();
    var result = collection.Find(filter).ToList();
    stopwatch.Stop();
    Console.WriteLine("Benchmark finished. Time elapsed: " + stopwatch.GetElapsedTime());
    Console.WriteLine("Totally " + result.Count + " documents found.");
    Console.WriteLine("Write 'back' to go back to main menu or 'exit' to exit.");
    Console.WriteLine("Write 'show(n)' to show first n documents.");
    Console.WriteLine("Write anything else to read again.");
    ShowResults(result);
}

void ShowResults(List<BsonDocument> result)
{
    while (true)
    {
        var input = Console.ReadLine();
        if (input == exitInput)
            Environment.Exit(0);
        if (input == backInput)
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
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(i + ":" + result[i]);
            }
        }
    }
}

void DeleteOperation()
{
    BsonDocument filter = GetFilterInput();
    Console.WriteLine("Starting benchmark.");
    var stopwatch = Stopwatch.Evaluate(() => collection.DeleteMany(filter));
    Console.WriteLine("Benchmark finished. Time elapsed: " + stopwatch.GetElapsedTime());
}

void UpdateOperation()
{
    BsonDocument filter = GetFilterInput();
    BsonDocument update = GetDocumentInput("Please enter update document: ");
    var stopwatch = Stopwatch.Evaluate(() => collection.UpdateMany(filter, update));
    Console.WriteLine("Benchmark finished. Time elapsed: " + stopwatch.GetElapsedTime());
}

void InsertOperation(IDocumentGenerator documentGenerator)
{
    int numOfDocuments = GetIntInput("Please enter number of documents to generate: ");
    Console.WriteLine("Starting benchmark.");
    Console.WriteLine("Documents Generating...");
    BsonDocument[] documents = documentGenerator.Generate(numOfDocuments);
    Console.WriteLine("Bulk Inserting...");
    var stopwatch = Stopwatch.Evaluate(() => collection.InsertMany(documents));
    Console.WriteLine("Benchmark finished. Time elapsed: " + stopwatch.GetElapsedTime());
    Console.WriteLine("Write 'back' to go back to main menu or 'exit' to exit.");
    Console.WriteLine("Write anything else to insert again.");
    var input = Console.ReadLine();
    if (input == exitInput)
        Environment.Exit(0);
    if (input == backInput)
        return;
    InsertOperation(documentGenerator);
}
