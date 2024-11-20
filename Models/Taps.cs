using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using GargMarbleServer.Models;

public class Tap
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Material { get; set; }

    public string Brand { get; set; }

    public int Quantity { get; set; }

    public List<string> Colors { get; set; }

    public List<string> Image { get; set; }

    public Dimensions Size { get; set; }

    public List<string> Customers { get; set; }

    public string Description { get; set; }

    public Specifications Specifications { get; set; }
}

