namespace MongoDBBenchmark;

public class Benchmark<T>
{
    private readonly Stopwatch _stopwatch = new();
    private readonly Func<T> _action;
    private readonly string _input; /* likely bson document string */
    private static Logger _logger = new("operations.log");
    
    public Benchmark(Func<T> action, string input)
    {
        _action = action;
        _input = input;
    }
    
    public T Run(Func<T, string> logMessage)
    {
        _stopwatch.Start();
        T val = _action();
        _stopwatch.Stop();
        string message = logMessage(val).Replace("{time}", _stopwatch.GetElapsedTime().ToString());
        message = message.Replace("{input}", _input);
        _logger.Log(message);
        return val;
    }
    
    public TimeSpan GetElapsedTime()
    {
        return _stopwatch.GetElapsedTime();
    }
}