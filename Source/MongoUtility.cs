using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;

namespace TingTango.Source
{
    internal static class MongoUtility
    {
        public static T BsonToDocument<T>(BsonDocument data)
        {
            return BsonSerializer.Deserialize<T>(data);
        }

        public static List<T> BsonToDocument<T>(List<BsonDocument> bsonList)
        {
            var dataList = new List<T>();
            foreach (var bsonDoc in bsonList)
                dataList.Add(BsonToDocument<T>(bsonDoc));

            return dataList;
        }

        public static IMongoCollection<BsonDocument> GetCollectionAsBsonDocument(IMongoDatabase database, string collectionNameLocal)
        {
            return database.GetCollection<BsonDocument>(collectionNameLocal);
        }

        public static IFindFluent<BsonDocument, BsonDocument> GetFilteredCollection(IMongoDatabase database, string collectionNameLocal, BsonDocument filter)
        {
            return GetCollectionAsBsonDocument(database, collectionNameLocal).Find(filter);
        }

        public static IFindFluent<BsonDocument, BsonDocument> GetFilteredCollection(IMongoDatabase database, string collectionNameLocal, FilterDefinition<BsonDocument> filter)
        {
            return GetCollectionAsBsonDocument(database, collectionNameLocal).Find(filter);
        }
    }
}