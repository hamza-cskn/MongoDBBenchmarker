namespace MongoDBBenchmark;

public enum Operations
{
    Insert,
    Update,
    Delete,
    Read
}

public static class OperationsExtensions
{
    public static Operations? FromString(string? input)
    {
        // todo try parse
        if (input == null)
            return null;
        return input.ToLower() switch
        {
            "insert" => Operations.Insert,
            "update" => Operations.Update,
            "delete" => Operations.Delete,
            "read" => Operations.Read,
            _ => null
        };
    }
}