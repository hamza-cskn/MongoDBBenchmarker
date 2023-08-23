namespace MongoDBBenchmark;

public class Stopwatch
{
    private long _start;
    private long _end;
    
    public static Stopwatch Evaluate(Action action)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        action();
        stopwatch.Stop();
        return stopwatch;
    }
    
    public void Start()
    {
        _start = DateTime.Now.Ticks;
    }
    
    public void Stop()
    {
        _end = DateTime.Now.Ticks;
    }
    
    public long GetElapsedTicks()
    {
        if (_end == 0)
            return DateTime.Now.Ticks - _start;
        return _end - _start;
    }
    
    public TimeSpan GetElapsedTime()
    {
        return TimeSpan.FromTicks(GetElapsedTicks());
    }
    
}