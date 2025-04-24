/* The Backend Module provides all the operations such as Create, Remove, DisplayItem, DisplayAll, AttachDocument
It performs all the user input operations, save all the items to FileStore, logs all user actions, and output
messages appropriate for the user to see in the consoleUI */
namespace Project.Backend
{
    using System;
    using Project.Data;
    using Project.Frontend;
    using Spectre.Console;
    using Project.Debugging;

    public static class Operations
    {
        // Create method that ask for user inputs, save it to files, and display messages
        public static void Create()
        {

            string name = AnsiConsole.Ask<string>("Enter item detail: ");
            string store = ConsoleUI.Ask("Enter store detail: ");
            string category = ConsoleUI.Ask("Enter category detail: ");
            string price = ConsoleUI.Ask("Enter price ($): ");
            string date = ConsoleUI.Ask("Enter purchase date (MM-DD-YYYY): ");

            string eachItem = $"{name} | {store} | {category} | {price} | {date} \n";

            DataStore.trackerList.Add(eachItem);
            Logs.LogFiles($"Item created: {eachItem}");
            FileStore.SaveFiles();
            ConsoleUI.Print("[green]Item successfully created.[/]");
        }

        // remove method that search for the item in the file, remove it and display message
        public static void RemoveItem(string name)
        {
            int idx = DataStore.trackerList.FindIndex(item => item.ToLower().StartsWith(name.ToLower() + " |"));

            if (idx >= 0)
            {
                string removedItem = DataStore.trackerList[idx];
                DataStore.trackerList.RemoveAt(idx);

                DataStore.docList.RemoveAll(doc => doc.ToLower().StartsWith(name.ToLower() + " |"));
                Logs.LogFiles($"Item Removed: {removedItem}");
                FileStore.SaveFiles();
                ConsoleUI.Print($"[green]Item successfully removed: {removedItem}[/]");
            } 
            
            // display item not found
            else {
                string errorMessage = $"Item {name} not found\n";
                File.AppendAllText("tracker_log.txt", errorMessage);
                ConsoleUI.Print("Item not found");
            }
        }

        // attach method that takes in one param name and search for the element in collection and add document details to it
        public static void AttachDocument(string name)
        {
            
            var itemStatus = DataStore.trackerList.FirstOrDefault(item => item.ToLower().StartsWith(name.ToLower() + " |"));

            if (itemStatus != null)
            {
                string itemType = ConsoleUI.Ask("Enter the document category ([bold yellow]Policy, Warranty[/]): ");
                string itemDescription = ConsoleUI.Ask("Description: ");
                string expireDate = ConsoleUI.Ask("Expire Date [red](MM-DD-YYYY)[/]: ");

                string eachDocument = $"{name} | {itemType} | {itemDescription} | {expireDate} \n";
                DataStore.docList.Add(eachDocument);
                FileStore.SaveFiles();

                Logs.LogFiles($"Document attached to {name} : {eachDocument}");
                ConsoleUI.Print("[bold green]Document attached successfully[/]");
                
            }
            else
            {
                ConsoleUI.Print("[red]Item Not Found![/]");
            }
        }

        // Display method that takes in one param name and find the item, and display the item and document details
        public static void DisplayItem(string name)
        {
            string itemInfo = DataStore.trackerList.Find(item => item.ToLower().StartsWith(name.ToLower() + " |"));

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
                ConsoleUI.PrintTable(itemTable);
                
                var docInfo = DataStore.docList.Where(doc => doc.ToLower().StartsWith(name.ToLower() + " |")).ToList();
                if (docInfo.Any())
                {
                    ConsoleUI.Print("[yellow]Attached Document:[/]");
                    var docTable = new Table();
                    docTable.AddColumn("Document_Name");
                    docTable.AddColumn("Document_Type");
                    docTable.AddColumn("Description:");
                    docTable.AddColumn("Expire_Date");

                    foreach (var doc in docInfo)
                    {
                        string[] docDetail = doc.Split('|');
                        docTable.AddRow(docDetail[0], docDetail[1], docDetail[2], docDetail[3]);
                    }
                    ConsoleUI.PrintTable(docTable);
                }
                else
                {
                    ConsoleUI.Print("[bold red]No Document Detail Available![/]");
                }
            }
            else
            {
                ConsoleUI.Print("[bold red]No Item Detail Available![/]");
            }
        }

        // Display method to output all the item and its document detail in a table format
        public static void DisplayAll()
        {

            if (DataStore.trackerList.Count == 0) {
                ConsoleUI.Print("No Item Found!");
                return;
            }

            var itemTable = new Table();
            itemTable.AddColumn("Item_ID");
            itemTable.AddColumn("item_name");
            itemTable.AddColumn("store_detail");
            itemTable.AddColumn("category");
            itemTable.AddColumn("price");
            itemTable.AddColumn("purchase_date");
            itemTable.AddColumn("document_type");
            itemTable.AddColumn("description");
            itemTable.AddColumn("expire_date");
            
            for (int i = 0; i < DataStore.trackerList.Count; i++) {
                string itemList = DataStore.trackerList[i];

                string[] itemDetail = itemList.Split('|');

                if (itemDetail.Length < 5) {
                    continue;
                }
                
                string item_name = itemDetail[0].Trim();
                string store_detail = itemDetail[1].Trim();
                string category = itemDetail[2].Trim();
                string price = itemDetail[3].Trim();
                string purchase_date = itemDetail[4].Trim();

                var docInfo = DataStore.docList.Find(doc => doc.ToLower().StartsWith(item_name.ToLower() + " |"));

                string doc_type = docInfo != null ? docInfo.Split('|')[1].Trim() : "Not Available";
                string doc_description = docInfo != null ? docInfo.Split('|')[2].Trim() : "Not Available";
                string doc_expire_date = docInfo != null ? docInfo.Split('|')[3].Trim() : "Not Available";

                if (docInfo != null) {
                    string[] docDetail = docInfo.Split('|');

                    if (docDetail.Length >= 4) {
                    }
                    else {
                        ConsoleUI.Print("not enuff info!");
                    }
                }

                itemTable.AddRow(
                    (i+1).ToString(),
                    item_name,
                    store_detail,
                    category,
                    price,
                    purchase_date,
                    doc_type,
                    doc_description,
                    doc_expire_date
                );
            }
            ConsoleUI.PrintTable(itemTable);
        }

    }
}