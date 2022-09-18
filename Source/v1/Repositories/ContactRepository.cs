using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using TingTango.Source.v1.Models;

namespace TingTango.Source.v1.Repositories;

public class ContactRepository
{
    private static readonly ContactRepository singleton;
    private static readonly string collectionName;
    private IMongoDatabase database;

    static ContactRepository()
    {
        singleton = new ContactRepository();
        collectionName = "contacts";
        singleton.database = Database.GetDBContext();
    }

    private ContactRepository()
    {
    }

    public static ContactRepository Instance()
    {
        return singleton;
    }

    public List<Contact> GetAll()
    {
        var filter = new BsonDocument();
        var contacts = MongoUtility.GetFilteredCollection(database, collectionName, filter).ToList();
        return MongoUtility.BsonToDocument<Contact>(contacts);
    }

    public List<Contact> GetAlike(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Regex("name", $"/.*{name}.*/");
        var contacts = MongoUtility.GetFilteredCollection(database, collectionName, filter).ToList();
        return MongoUtility.BsonToDocument<Contact>(contacts);
    }

    public Contact GetExact(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("name", name);
        var contact = MongoUtility.GetFilteredCollection(database, collectionName, filter).FirstOrDefault();
        if (contact == null)
            return null;
        return MongoUtility.BsonToDocument<Contact>(contact);
    }

    public void Create(Contact contact)
    {
        if (GetExact(contact.Name) != null)
            throw new ApplicationException(ApplicationMessages.DuplicateFound);

        new ObjectId();
        MongoUtility.GetCollectionAsBsonDocument(database, collectionName).InsertOne(contact.ToBsonDocument());
    }

    public void Update(string name, Contact contact)
    {
        if (GetExact(name) == null)
            throw new ApplicationException(ApplicationMessages.NoMatchFound);

        var filter = Builders<BsonDocument>.Filter.Eq("name", name);
        var update = Builders<BsonDocument>.Update.Set("name", contact.Name);
        MongoUtility.GetCollectionAsBsonDocument(database, collectionName).UpdateMany(filter, update);
    }

    public void Delete(string name)
    {
        if (GetExact(name) == null)
            throw new ApplicationException(ApplicationMessages.NoMatchFound);

        var filter = Builders<BsonDocument>.Filter.Eq("name", name);
        MongoUtility.GetCollectionAsBsonDocument(database, collectionName).DeleteMany(filter);
    }
}