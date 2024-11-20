using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using GargMarbleServer.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }// MongoDB uses string as the default primary key
    public string First { get; set; }
    public string Last { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Number { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string Pincode { get; set; }
    public bool Confirmed { get; set; }
}

public class RegisterRequest
{
    public string First { get; set; }
    public string Last { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RePassword { get; set; }
    public string Number { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string Pincode { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
