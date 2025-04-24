/* The Data Module has FileStore class with two methods to load data from itemFile and documentFile into DataStore
and SaveFiles in memory data lits. These logics allow read and writes files with persistence data */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Project.Data
{
    public static class FileStore
    {
        private static string itemFile = "tracker_items.txt";
        private static string documentFile = "tracker_documents.txt";

        public static void LoadFiles()
        {
            DataStore.trackerList = File.Exists(itemFile) ? File.ReadAllLines(itemFile).ToList() : new List<string>();
            DataStore.docList = File.Exists(documentFile) ? File.ReadAllLines(documentFile).ToList() : new List<string>();
        }

        public static void SaveFiles()
        {
            File.WriteAllLines(itemFile, DataStore.trackerList);
            File.WriteAllLines(documentFile, DataStore.docList);
        }
    }
}