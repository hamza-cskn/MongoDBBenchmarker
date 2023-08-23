// See https://aka.ms/new-console-template for more information

// get input

using MongoDB.Bson;
using MongoDBBenchmark;
using MongoDBBenchmark.Client;
using static MongoDBBenchmark.InputManager;

Console.WriteLine("Welcome to Mongo DB Benchmark.");
Console.WriteLine("Write 'exit' or '^C' to exit.");

Client client = new Client(GetConnectionString(), GetDatabaseName(), GetCollectionName());
Start();
void Start()
{
    Operations operation = GetOperation();
    switch (operation)
    {
        case Operations.Insert:
            InsertOperation();
            break;
        case Operations.Read:
            ReadOperation();
            break;
        case Operations.Update:
            //UpdateOperation();
            break;
        case Operations.Delete:
            //DeleteOperation();
            break;
        default:
            throw new ArgumentOutOfRangeException("invalid operation type: " + operation);
    }
    
    Start();
}

void ReadOperation()
{
    BsonDocument filter = GetFilter();
    var result = client.Read(filter);
    Console.WriteLine("Result: ");
    foreach (var document in result)
    {
        Console.WriteLine("-" + document);
    }
}

void InsertOperation()
{
    int numOfDocuments = GetNumberOfDocuments();
    Func<int, BsonDocument> documentGenerator = GetDocumentGenerator();
    BsonDocument[] documents = new BsonDocument[numOfDocuments];
    
    Console.WriteLine("Starting benchmark...");
    Console.WriteLine("Number of documents: " + numOfDocuments);
    for (int i = 0; i < numOfDocuments; i++)
    {
        documents[i] = documentGenerator(i);
    }
    Console.WriteLine("Generated Documents: ");
    foreach (var document in documents)
    {
        Console.WriteLine("-" + document);
    }
}
