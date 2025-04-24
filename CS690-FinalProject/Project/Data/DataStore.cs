// DataStore connects with FileStore and store data to load, save, and use with CRUD methods. 
namespace Project.Data
{
    public static class DataStore
    {
        public static List<string> trackerList{get;set;} = new List<string>();
        public static List<string> docList{get;set;} = new List<string>();
    }
}