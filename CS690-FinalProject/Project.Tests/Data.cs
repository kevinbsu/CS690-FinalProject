// This module is to test Data Module to validate all loadFile, SaveFile, and check if DatContext contains all the added items.
using Project.Data;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Project.Tests
{
    public class FileStoreTests
    {
        private void CleanUpFiles()
        {
            if (File.Exists("tracker_items.txt"))
            {
                File.Delete("tracker_items.txt");
            }

            if (File.Exists("tracker_documents.txt"))
            {
                File.Delete("tracker_documents.txt");
            }
        }

        [Fact]
        public void LoadFile_Validation()
        {
            DataStore.trackerList.Clear();

            // File.WriteAllLines("tracker_items.txt", new[] {"test123", "item2"});
            File.WriteAllLines("tracker_documents.txt", new List<string> {"doc1", "doc2"});

            FileStore.LoadFiles();

            // Assert.Equal(new List<string> {"item1", "item2"}, DataStore.trackerList;
            Assert.Equal(new List<string> {"doc1", "doc2"}, DataStore.docList);

            CleanUpFiles();
        }

        [Fact]
        public void LoadFiles_NotExist()
        {
            CleanUpFiles();

            FileStore.LoadFiles();

            Assert.Empty(DataStore.trackerList);
            Assert.Empty(DataStore.docList);
        }

        [Fact]
        public void SaveFiles_Validation()
        {
            DataStore.trackerList = new List<string> {"item1", "item2"};
            DataStore.docList = new List<string> {"doc1", "doc2"};

            FileStore.SaveFiles();

            Assert.True(File.Exists("tracker_items.txt"));
            Assert.True(File.Exists("tracker_documents.txt"));
            Assert.Equal(new List<string> {"item1", "item2"}, File.ReadAllLines("tracker_items.txt"));
            Assert.Equal(new List<string> {"doc1", "doc2"}, File.ReadAllLines("tracker_documents.txt"));

            CleanUpFiles();
        }

        [Fact]
        public void DataStore_ItemValidation()
        {
            Assert.NotNull(DataStore.trackerList);
        }

        [Fact]
        public void DataStore_DocumentValidation()
        {
            Assert.NotNull(DataStore.docList);
        }

        [Fact]
        public void DataStore_AddItem()
        {
            DataStore.trackerList.Clear();
            string item = "test123";

            DataStore.trackerList.Add(item);
            Assert.Single(DataStore.trackerList);
            Assert.Equal("test123", DataStore.trackerList[0]);
        }

        [Fact]
        public void DataStore_AddDoc()
        {
            DataStore.docList.Clear();
            var documents = new[] {"doc1"};
            foreach (var item in documents)
            {
                DataStore.docList.Add(item);
            }
            Assert.Single(DataStore.docList);
            Assert.Contains("doc1", DataStore.docList);
        }
    }
}