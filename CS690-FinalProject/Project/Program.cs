namespace Project;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{   
    // Declare variables and assign it with the file path
    static string logFile = "tracker_log.txt";
    static string itemFile = "tracker_items.txt";
    static string documentFile = "tracker_documents.txt";

    // Init list variables and invoke the methods
    static List<string> trackerList = LoadTrackerFile();
    static List<string> docList = LoadDocumentFile();

    // Use Spectre Console to output welcome message, user_name, and invoke ShowMenu method
    static void Main(string[] args)
    {
        AnsiConsole.MarkupLine("[bold green]Personal Inventory Tracker CLI![/]");
        var userName = AnsiConsole.Ask<string>("Enter your [bold red]user_name[/]: ");
        AnsiConsole.MarkupLine($"Welcome to [bold red]{userName}'s[/] Personal Inventory Tracker System!");
        ShowMenu();
    }

    // Create a AnsiConole prompt to loop through the different choices and take userInput
    static void ShowMenu()
    {
        while (true)
        {
            var userInput = AnsiConsole.Prompt(
                                     new SelectionPrompt<string>()
                                        .Title("\nSelect your choice: ")
                                        .AddChoices( new[] {
                                            "Create Item",
                                            "Remove Item",
                                            "Attach Document",
                                            "Display Item Details",
                                            "Exit Program"
                                    }));

            AnsiConsole.MarkupLine($"[bold]You selected[/] [bold green]{userInput}![/]");
            
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
                case "Exit Program":
                    Save();
                    AnsiConsole.MarkupLine("[bold green]Program Ended![/]");
                    return;
                default:
                    AnsiConsole.MarkupLine("[bold red]Not valid![/]");
                    break;
            }
        }
    }

    // CreateItem method to take user inputs, store in variables, and append it to the files.
    static void CreateItem()
    {
        string name = AnsiConsole.Ask<string>("Enter item detail: ");
        string store = AnsiConsole.Ask<string>("Enter store detail: ");
        string category = AnsiConsole.Ask<string>("Enter category detail: ");
        string price = AnsiConsole.Ask<string>("Enter price ($): ");
        string date = AnsiConsole.Ask<string>("Enter purchase date (MM-DD-YYYY): ");


        string eachItem = $"{name} | {store} | {category} | {price} | {date} \n";
        trackerList.Add(eachItem);

        string eachLog = $"Log for Created Item: {DateTime.Now}: {eachItem}\n";
        File.AppendAllText(logFile, eachLog);
        Save();

        AnsiConsole.MarkupLine("[bold green]Item created successfully![/]");
    }

    // Create RemoveItem method to compare user inputs and search through the idx to find matching item
    // and remove it from the file
    static void RemoveItem()
    {
        string name = AnsiConsole.Ask<string>("Enter name of item to remove: ");
        int idx = trackerList.FindIndex(item => item.ToLower().StartsWith(name.ToLower() + " |"));

        if (idx >= 0)
        {
            string removedItem = trackerList[idx];
            trackerList.RemoveAt(idx);

            string eachLog = $"Log for Remove Item : {DateTime.Now} {removedItem}\n";
            File.AppendAllText(logFile, eachLog);

            Save();
            AnsiConsole.MarkupLine("[bold green]Item removed successfully![/]");
        } 
        
        else {
            AnsiConsole.MarkupLine("[bold red]Item Not Found![/]");
        }
    }

    // Create AttachDocument method to check items in the existing list, 
    // if found, add user inputs to the document details and to file
    static void AttachDocument()
    {
        string name = AnsiConsole.Ask<string>("Enter the document details: ");
        bool itemStatus = trackerList.Exists(item => item.ToLower().StartsWith(name.ToLower() + " |"));

        if (!itemStatus)
        {
            AnsiConsole.MarkupLine("[bold red]Not found![/]");
            return;
        }

        string itemType = AnsiConsole.Ask<string>("Enter the document category ([bold yellow]Policy, Warranty[/]): ");
        string itemDescription = AnsiConsole.Ask<string>("Description: ");
        string expireDate = AnsiConsole.Ask<string>("Expire Date [red](MM-DD-YYYY)[/]: ");

        string eachDocument = $"{name} | {itemType} | {itemDescription} | {expireDate} \n";
        File.AppendAllText(documentFile, eachDocument);
        docList.Add(eachDocument);

        string eachLog = $"Log for Document : {DateTime.Now} | {name} | {itemType} | {itemDescription}\n";
        File.AppendAllText(logFile, eachLog);

        AnsiConsole.MarkupLine("[bold green]AttachDocument Successful![/]");
    }

    // Create DisplayItem method to ask for user input and find 
    // and display the item and its document details in a table format
    static void DisplayItem()
    {
        string name = AnsiConsole.Ask<string>("Enter item name: ");
        string itemInfo = trackerList.Find(item => item.ToLower().StartsWith(name.ToLower() + " |"));

        if (itemInfo != null)
        {
            string[] itemDetail = itemInfo.Split ('|');
            var itemTable = new Table();
            itemTable.AddColumn("Item_Name");
            itemTable.AddColumn("Store_Detail");
            itemTable.AddColumn("Category");
            itemTable.AddColumn("Price($)");
            itemTable.AddColumn("Purchase_Date");

            itemTable.AddRow(itemDetail[0], itemDetail[1], itemDetail[2], itemDetail[3], itemDetail[4]);
            AnsiConsole.Write(itemTable);
            
            var docInfo = docList.Find(doc => doc.ToLower().StartsWith(name.ToLower() + " |"));
            if (docInfo != null)
            {
                string [] docDetail = docInfo.Split('|');

                var docTable = new Table();
                docTable.AddColumn("Document_Name");
                docTable.AddColumn("Document_Type");
                docTable.AddColumn("Description:");
                docTable.AddColumn("Expire_Date");

                docTable.AddRow(docDetail[0], docDetail[1], docDetail[2], docDetail[3]);
                AnsiConsole.Write(docTable);
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]No Document Detail Available![/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]No Item Detail Available![/]");
        }
    }

    // Create LoadTrackerFile and LoadDocumentFile methods to read the data from file and load as list
    static List<string> LoadTrackerFile()
    {
        if (File.Exists(itemFile))
        {
            return new List<string>(File.ReadAllLines(itemFile));
        }
        return new List<string>();
    }

    static List<string> LoadDocumentFile()
    {
        if (File.Exists(documentFile))
        {
            return new List<string>(File.ReadAllLines(documentFile));
        }
        return new List<string>();
    }

    // Create Save method 
    static void Save()
    {
        File.WriteAllLines(itemFile, trackerList);
        File.WriteAllLines(documentFile, docList);
    }
}