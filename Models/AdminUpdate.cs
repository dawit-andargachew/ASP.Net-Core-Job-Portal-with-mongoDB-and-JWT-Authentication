namespace web.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class AdminUpdate
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string name { get; set; }
    public string? lastName { get; set; }
    public string phone { get; set; }
    public string email { get; set; }

    public AdminUpdate(string lastName, string name, string phone, string email)
    {
        this.lastName = lastName;
        this.email = email;
        this.name = name;
        this.phone = phone;
    }
}
