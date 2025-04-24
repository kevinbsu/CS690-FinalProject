/* The FrontEnd Module has classUI with several method to ask for user inputs, print message, 
print header, and print table with Spectre Console formatting.*/
using Spectre.Console;
namespace Project.Frontend
{
    public static class ConsoleUI
    {
        public static string Ask(string prompt)
        {
            return AnsiConsole.Ask<string>(prompt);
        }

        public static void Print(string message)
        {
            AnsiConsole.MarkupLine(message);
        }

        public static void PrintHeader(string title)
        {
            AnsiConsole.MarkupLine($"[bold green]{title}[/]");
        }

        public static void PrintTable(Table table)
        {
            AnsiConsole.Write(table);
        }
    }
}