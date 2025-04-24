/* This module is to keep track of all operations from the user inputs into file: tracker_log.txt */
using System;
using System.IO;
namespace Project.Debugging
{
    public static class Logs
    {
        private static string logFile = "tracker_log.txt";

        public static void LogFiles(string message)
        {
            string logging = $"{DateTime.Now}: {message}{Environment.NewLine}";
            File.AppendAllText(logFile, logging);
        }
    }
}