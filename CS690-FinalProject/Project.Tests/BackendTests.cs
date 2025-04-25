//This is a test module for Backend to check all Create, Remove, AttachDocument, DisplayItem, DisplayAll
using Xunit;
using System.IO;
using Project.Backend;
using Project.Data;
using System.Text.RegularExpressions;

namespace Project.Tests.Backend
{
    using Spectre.Console;
    public class OperationsTests
    {

        private string filePath = "tracker_log.txt";
        private string outputPath = "tracker_item.txt";

        public OperationsTests()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }

        [Fact]
        public void RemoveItem_Validation()
        {
            DataStore.trackerList.Clear();
            string name = "test123";
            var item = new List<string> { "test123 | store | category | 9.99 | 12-12-2012" };
            DataStore.trackerList = item;

            Operations.RemoveItem(name);

            Assert.DoesNotContain("test123", DataStore.trackerList);

            string[] logContent = File.ReadAllLines(filePath);
            bool containsLogMessage = logContent.Any(line => line.Contains($"Item Removed: {name}"));
            Assert.True(containsLogMessage, "Log file with unexpected messages!");

        }

        [Fact]
        public void RemoveItem_NotFound()
        {
            DataStore.trackerList.Clear();
            string name = "test123";
            var item = new List<string> {"existingItem | store | category | 9.99 | 12-12-2012"};
            DataStore.trackerList = item;

            File.WriteAllText(filePath, string.Empty);

            Operations.RemoveItem(name);

            Assert.Contains("existingItem | store | category | 9.99 | 12-12-2012", DataStore.trackerList);

            string[] logContent = File.ReadAllLines(filePath);
            string expectedErrorMessage = $"Item {name} not found";

            Assert.Contains(expectedErrorMessage, logContent);
        }

        [Fact]
        public void CreateItem_Valiation()
        {
            DataStore.trackerList.Clear();

            string name = "test123";
            string store = "store123";
            string category = "category123";
            decimal price = 9.99m;
            string date = "01-01-1999";
    
            string eachItem = $"{name} | {store} | {category} | {price} | {date}";
            DataStore.trackerList.Add(eachItem);

            var result = DataStore.trackerList;
            Assert.Contains(eachItem, result);
            Assert.Single(result);
        } 

        [Fact]
        public void CreateItem_NoDuplicatedAdded()
        {
            DataStore.trackerList.Clear();

            string name = "test123";
            string store = "store123";
            string category = "category123";
            decimal price = 9.99m;
            string date = "01-01-1999";
            string eachItem = $"{name} | {store} | {category} | {price} | {date}";
            DataStore.trackerList.Add(eachItem);

            // string duplicated_item = "test123";
            // var confirm = false;
            bool duplicated = DataStore.trackerList.Exists(item => item.ToLower().StartsWith(name.ToLower() + " |"));

            Assert.Single(DataStore.trackerList);
        }


        [Fact]
        public void DisplayItem_ItemNotMatch()
        {
            DataStore.trackerList.Clear();

            string name = "test123";
            string store = "store123";
            string category = "category123";
            decimal price = 9.99m;
            string date = "01-01-1999";
            string itemToAdd = $"{name} | {store} | {category} | {price} | {date}";
            DataStore.trackerList.Add(itemToAdd);

            var output = new StringWriter();
            Console.SetOut(output);

            Operations.DisplayItem(name);

            var result = output.ToString();

            Console.WriteLine("result: ");
            Console.WriteLine(result);

            Assert.DoesNotContain("apple", result);
            Assert.DoesNotContain("apple123", result);
            Assert.DoesNotContain("category999", result);
            Assert.DoesNotContain("15.99", result);
            Assert.DoesNotContain("05-05-1900", result);
        }

        [Fact]
        public void DisplayItem_FailMessage()
        {
            string name = "test123";
            DataStore.trackerList.Clear();

            var output = new StringWriter();
            Console.SetOut(output);

            Operations.DisplayItem(name);

            var result = output.ToString();

            Console.WriteLine("CapOut: ");
            Console.WriteLine(result);
            Assert.DoesNotContain("Item not found", result.Trim());
        }

        [Fact]
        public void DisplayAll_ItemNotFound()
        {
             DataStore.trackerList.Clear();

            string name = "test123";
            string store = "store123";
            string category = "category123";
            decimal price = 9.99m;
            string date = "01-01-1999";
            string itemToAdd = $"{name} | {store} | {category} | {price} | {date}";
            DataStore.trackerList.Add(itemToAdd);

            var output1 = new StringWriter();
            Console.SetOut(output1);

            Operations.DisplayAll();

            var result1 = output1.ToString();

            Console.WriteLine("CapOutput: ");
            Console.WriteLine(result1);
            Assert.DoesNotContain("test1234!", result1);
            Assert.DoesNotContain("store1234", result1);
            Assert.DoesNotContain("category1234", result1);
            Assert.DoesNotContain("11.99", result1);
            Assert.DoesNotContain("02-02-2001", result1);
        }

        [Fact]
        public void AttachDocument_Validation()
        {
            string name = "not_found";
            DataStore.trackerList.Clear();

            string itemType = "Policy";
            string itemDesc = "Product refundable";
            string expireDate = "01-01-1999";
                
            string Input = $"{name}\n{itemType}\n{itemDesc}\n{expireDate}\n";
            var stringReader = new StringReader(Input);
            Console.SetIn(stringReader);

            var output = new StringWriter();
            Console.SetOut(output);

            Operations.AttachDocument(name);
            Assert.DoesNotContain(name, DataStore.docList);
        }
    }
}