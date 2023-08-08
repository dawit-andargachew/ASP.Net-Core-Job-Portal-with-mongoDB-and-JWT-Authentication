namespace web.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Employer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string name { get; set; }
    public string? lastName { get; set; }
    public string? password { get; set; }
    public string phone { get; set; }
    public string email { get; set; }
    public string role = "employer";

    public Employer(
        string id,
        string lastName,
        string password,
        string name,
        string phone,
        string email
    )
    {
        this.Id = id;
        this.password = password;
        this.lastName = lastName;
        this.email = email;

        this.name = name;
        this.phone = phone;
    }
}
