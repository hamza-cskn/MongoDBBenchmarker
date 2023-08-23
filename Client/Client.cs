using MongoDB.Bson;

namespace MongoDBBenchmark.Client;
using MongoDB.Driver;

public class Client
{
    private MongoClient _client;
    private IMongoDatabase _database;
    private IMongoCollection<BsonDocument> _collection;
    public Client(string connectionString, string database, string collection)
    {
        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase(database);
        _collection = _database.GetCollection<BsonDocument>(collection);
        var result = _collection.Find(Builders<BsonDocument>.Filter.Eq("name", "hamza")).ToList();
        result.ForEach(x => Console.WriteLine(x));
    }
    
    public void Insert(BsonDocument document)
    {
        _collection.InsertOne(document);
    }
    
    public List<BsonDocument> Read(BsonDocument filter)
    {
        return _collection.Find(Builders<BsonDocument>.Filter.Eq("name", "hamza")).ToList();
    }
}
