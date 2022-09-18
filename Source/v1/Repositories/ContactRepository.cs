using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using TingTango.Source.v1.Models;

namespace TingTango.Source.v1.Repositories;

public class ContactRepository
{
    private static readonly ContactRepository Singleton;
    private static readonly string CollectionName;
    private IMongoDatabase _database;

    static ContactRepository()
    {
        Singleton = new ContactRepository();
        CollectionName = "contacts";
        Singleton._database = Database.GetDbContext();
    }

    private ContactRepository()
    {
    }

    public static ContactRepository Instance()
    {
        return Singleton;
    }

    public List<Contact> GetAll()
    {
        var filter = new BsonDocument();
        var contacts = MongoUtility.GetFilteredCollection(_database, CollectionName, filter).ToList();
        return MongoUtility.BsonToDocument<Contact>(contacts);
    }

    public List<Contact> GetAlike(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Regex("name", $"/.*{name}.*/");
        var contacts = MongoUtility.GetFilteredCollection(_database, CollectionName, filter).ToList();
        return MongoUtility.BsonToDocument<Contact>(contacts);
    }

    public Contact GetExact(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("name", name);
        var contact = MongoUtility.GetFilteredCollection(_database, CollectionName, filter).FirstOrDefault();
        return contact == null ? null : MongoUtility.BsonToDocument<Contact>(contact);
    }

    public void Create(Contact contact)
    {
        if (GetExact(contact.Name) != null)
            throw new ApplicationException(ApplicationMessages.DuplicateFound);

        MongoUtility.GetCollectionAsBsonDocument(_database, CollectionName).InsertOne(contact.ToBsonDocument());
    }

    public void Update(string name, Contact contact)
    {
        if (GetExact(name) == null)
            throw new ApplicationException(ApplicationMessages.NoMatchFound);

        var filter = Builders<BsonDocument>.Filter.Eq("name", name);
        var update = Builders<BsonDocument>.Update.Set("name", contact.Name);
        MongoUtility.GetCollectionAsBsonDocument(_database, CollectionName).UpdateMany(filter, update);
    }

    public void Delete(string name)
    {
        if (GetExact(name) == null)
            throw new ApplicationException(ApplicationMessages.NoMatchFound);

        var filter = Builders<BsonDocument>.Filter.Eq("name", name);
        MongoUtility.GetCollectionAsBsonDocument(_database, CollectionName).DeleteMany(filter);
    }
}