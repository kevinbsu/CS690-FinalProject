/* Program Module has Main, ShowMenu, and operations methods. The main loads files, print header and show 
menu options with several operations. ShowMenu has several switch cases depending on user inputs */
namespace Project
{
    using Spectre.Console;
    using Project.Frontend;
    using Project.Backend;
    using Project.Data;
    using Project.Debugging;
    class Program
    {
        static void Main(string[] args)
        {
            FileStore.LoadFiles();

            ConsoleUI.PrintHeader("Personal Inventory Tracker CLI");

            var userName = AnsiConsole.Ask<string>("Enter your [bold red]user_name[/]: ");
            AnsiConsole.MarkupLine($"[bold yellow]Welcome[/] to [bold red]{userName}'s[/] [bold yellow]Personal Inventory Tracker System![/]");
            
            ShowMenu();
        }

        static void ShowMenu()
        {
            bool command = false;
            while (!command)
            {
                var userInput = AnsiConsole.Prompt(
                                        new SelectionPrompt<string>()
                                            .Title("\n[yellow]Select your choice:[/] ")
                                            .AddChoices( new[] {
                                                "Create Item",
                                                "Remove Item",
                                                "Attach Document",
                                                "Display Item Details",
                                                "Display All",
                                                "Exit Program"
                                        }));

                Console.Clear();
                
                switch(userInput)
                {
                    case "Create Item":
                        CreateItem();
                        break;
                    case "Remove Item":
                        RemoveItem();
                        break;
                    case "Attach Document":
                        AttachDocument();
                        break;
                    case "Display Item Details":
                        DisplayItem();
                        break;
                    case "Display All":
                        Operations.DisplayAll();
                        break;
                    case "Exit Program":
                        AnsiConsole.MarkupLine("[bold green]Program Ended![/]");
                        return;
                        command = true;
                        break;
                    default:
                        ConsoleUI.Print("[bold red]Not valid![/]");
                        break;
                }
            }
        }
        
        static void CreateItem()
        {
            string name = ConsoleUI.Ask("Enter item detail: ");

            bool duplicated = DataStore.trackerList.Exists(item => item.ToLower().StartsWith(name.ToLower() + " |"));

            //check for duplicated when user input item name
            if (duplicated) {
                var confirm = AnsiConsole.Confirm(
                    $"Item {name} already exists! Do you want to add duplicate?",
                    defaultValue: false
                );

                if (!confirm) {
                    AnsiConsole.MarkupLine($"[bold red]Item not added![/]");
                    return;
                }
            }

            string store = ConsoleUI.Ask("Enter store detail: ");
            string category = ConsoleUI.Ask("Enter category detail: ");
            string price = ConsoleUI.Ask("Enter price [yellow]($)[/]: ");
            string date = ConsoleUI.Ask("Enter purchase date [red](MM-DD-YYYY)[/]: ");

            string eachItem = $"{name} | {store} | {category} | {price} | {date} \n";
            DataStore.trackerList.Add(eachItem);
            FileStore.SaveFiles();
            ConsoleUI.Print("[green]Item successfully created.[/]");
        }

        static void RemoveItem()
        {
            string name = ConsoleUI.Ask("Enter the item name to remove: ");
            Operations.RemoveItem(name);
        }

        static void AttachDocument()
        {
            string name = ConsoleUI.Ask("Enter item name to attach doc: ");
            Operations.AttachDocument(name);
        }

        static void DisplayItem()
        {
            string name = ConsoleUI.Ask("Enter item name to display: ");
            Operations.DisplayItem(name);
        }
    }
}