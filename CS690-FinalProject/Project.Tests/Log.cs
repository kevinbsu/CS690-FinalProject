// This is a test module for Log to validate if tracker_log file contains the appropriate logged message.
using System;
using System.IO;
using Xunit;
using Project.Debugging;

public class LogsTests
{
    [Fact]
    public void Logs_Validation()
    {
        string file = "tracker_log.txt";
        if (File.Exists(file))
        {
            File.Delete(file);
        }

        string msg = "logs testing123";
        Logs.LogFiles(msg);

        string content = File.ReadAllText(file);
        Assert.Contains(msg, content);
    }
}