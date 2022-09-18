using MongoDB.Driver;

namespace TingTango.Source.v1;

internal static class Database
{
    public static IMongoDatabase GetDbContext()
    {
        var client = new MongoClient(AppConstant.ConnectionString);
        var database = client.GetDatabase(AppConstant.DbName);
        return database;
    }
}