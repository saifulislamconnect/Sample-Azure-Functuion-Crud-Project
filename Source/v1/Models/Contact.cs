using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TingTango.Source.v1.Models;

public class Contact
{
    /// <summary>
    ///     Auto generated ObjectId uniquely identified mongo
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId _id { get; set; }

    /// <summary>
    ///     Contact name. Systematically considered unique
    /// </summary>
    public string Name { get; set; }
}