namespace MongoDBBenchmark;

using System;
using System.IO;

public class Logger
{
    private string logFilePath;

    public Logger(string logFilePath)
    {
        this.logFilePath = logFilePath;
    }

    public void Log(string message)
    {
        try
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                writer.WriteLine(logEntry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }
}