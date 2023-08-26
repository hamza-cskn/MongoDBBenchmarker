using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBBenchmark.Generator;

namespace MongoDBBenchmark;

public class Client
{

    private IMongoCollection<BsonDocument> _collection;

    public void Connect(string? connectionString, string? databaseName, string? collectionName)
    {
        if (connectionString == null || databaseName == null || collectionName == null)
        {
            Console.WriteLine("Connection string, database name and collection name must be provided.");
            Console.WriteLine("Please ensure that you have provided these environment variables:");
            Console.WriteLine("BENCHMARK_DATABASE_NAME, BENCHMARK_COLLECTION_NAME, BENCHMARK_CONNECTION_STRING");
            Environment.Exit(0);
        }
        MongoClient client;
        try {
            client = new MongoClient(connectionString);
        } catch (Exception e) {
            Console.WriteLine("Could not connected to the database.");
            Console.WriteLine(e.Message);
            return;
        }
        client.GetDatabase(databaseName).CreateCollection(collectionName);
        _collection =  client
            .GetDatabase(databaseName)
            .GetCollection<BsonDocument>(collectionName);
    }

    public List<BsonDocument> Read(BsonDocument filter)
    {
        Console.WriteLine("Starting benchmark.");
        var benchmark = new Benchmark<List<BsonDocument>>(
            () => _collection.Find(filter).ToList(),
            filter.ToString());
        List<BsonDocument> result = benchmark.Run(res => "READ - Count: " + res.Count+ ", Time: {time}, Filter: {input}");
        Console.WriteLine("Benchmark finished. Time elapsed: " + benchmark.GetElapsedTime());
        Console.WriteLine("Totally " + result.Count + " documents found.");
        return result;
    }

    public void Delete(BsonDocument filter)
    {
        Console.WriteLine("Starting benchmark.");
        var benchmark = new Benchmark<DeleteResult>(
            () => _collection.DeleteMany(filter),
            filter.ToString());
        benchmark.Run(res => "DELETE - Count: " + res.DeletedCount + ", Time {time}, Filter: {input}");
        Console.WriteLine("Benchmark finished. Time elapsed: " + benchmark.GetElapsedTime());

    }

    public void Update(BsonDocument filter, BsonDocument update)
    {
        var benchmark = new Benchmark<UpdateResult>(
            () => _collection.UpdateMany(filter, update),
            filter.ToString());
        benchmark.Run(res => "UPDATE - Count: " + res.ModifiedCount + ", Time {time}, Filter: {input}, Updated: " + update);
        Console.WriteLine("Benchmark finished. Time elapsed: " + benchmark.GetElapsedTime());

    }

    public void Insert(int numberOfDocuments, IDocumentGenerator documentGenerator)
    {
        Console.WriteLine("Documents Generating...");
        BsonDocument[] documents = documentGenerator.Generate(numberOfDocuments);
        Console.WriteLine("Bulk Inserting...");
        var benchmark = new Benchmark<object>(
            () =>
            {
                _collection.InsertMany(documents);
                return null;
            },
            documentGenerator.GetRawTemplate());
        benchmark.Run(x => "INSERT - Count: " + numberOfDocuments + ", Time: {time}, Template: {input}");
        Console.WriteLine("Inserted " + numberOfDocuments + " documents.");
        Console.WriteLine("Benchmark finished. Time elapsed: " + benchmark.GetElapsedTime());
    }

}