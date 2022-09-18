using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace TingTango.Source;

internal static class MongoUtility
{
    public static T BsonToDocument<T>(BsonDocument data)
    {
        return BsonSerializer.Deserialize<T>(data);
    }

    public static List<T> BsonToDocument<T>(IEnumerable<BsonDocument> bsonList)
    {
        return bsonList.Select(BsonToDocument<T>).ToList();
    }

    public static IMongoCollection<BsonDocument> GetCollectionAsBsonDocument(IMongoDatabase database,
        string collectionNameLocal)
    {
        return database.GetCollection<BsonDocument>(collectionNameLocal);
    }

    public static IFindFluent<BsonDocument, BsonDocument> GetFilteredCollection(IMongoDatabase database,
        string collectionNameLocal, BsonDocument filter)
    {
        return GetCollectionAsBsonDocument(database, collectionNameLocal).Find(filter);
    }

    public static IFindFluent<BsonDocument, BsonDocument> GetFilteredCollection(IMongoDatabase database,
        string collectionNameLocal, FilterDefinition<BsonDocument> filter)
    {
        return GetCollectionAsBsonDocument(database, collectionNameLocal).Find(filter);
    }
}