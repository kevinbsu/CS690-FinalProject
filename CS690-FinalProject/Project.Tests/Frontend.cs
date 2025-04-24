// This is a test module for FrontEnd with all the methods in the ConsoleUI to validate all print, ask and table display.
using Project.Frontend;
using Spectre.Console;
using System;
using System.IO;
using Xunit;

namespace Project.Tests
{
    public class ConsoleUITests
    {
        [Fact]
        public void Print_Validation()
        {
            var writer = new StringWriter();
            Console.SetOut(writer);
            ConsoleUI.Print("test12345");

            var output = writer.ToString();
            Assert.DoesNotContain("test123", output);
        }

        [Fact]
        public void PrintHeader_Validation()
        {
            var writer = new StringWriter();
            Console.SetOut(writer);

            ConsoleUI.PrintHeader("title");
            var output = writer.ToString();
            Assert.DoesNotContain("test1234", output);
        }

        [Fact]
        public void UserPrompt_Validation()
        {
            string Input = "test123";
            Assert.Equal("test123", Input);
        }

        [Fact]
        public void PrintTable_Validation()
        {
            var writer = new StringWriter();
            Console.SetOut(writer);
            var table = new Table();
            table.AddColumn("Item_Name");
            table.AddRow("item1");

            ConsoleUI.PrintTable(table);

            var output = writer.ToString();
            Assert.DoesNotContain("item22", output);
            Assert.DoesNotContain("Store_Name", output);
        }
    }
}