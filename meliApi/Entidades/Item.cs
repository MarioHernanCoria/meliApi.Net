using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class Item
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string Title { get; set; }
    public string CategoryId { get; set; }
    public decimal Price { get; set; }

}
