using MongoDB.Driver;

namespace TingTango.Source.v1
{
    internal static class Database
    {
        public static IMongoDatabase GetDBContext()
        {
            var client = new MongoClient(AppConstant.connectionString);
            var database = client.GetDatabase(AppConstant.dbName);
            return database;
        }
    }
}
