namespace web.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class UpdateModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string name { get; set; }
    public string address { get; set; }
    public string profession { get; set; }
    public string? lastName { get; set; }

    public string phone { get; set; }
    public string email { get; set; }
    public string shortBio { get; set; }

    public UpdateModel(
        string profession,
        string lastName,
        string name,
        string shortBio,
        string phone,
        string email,
        string address
    )
    {
        this.profession = profession;

        this.lastName = lastName;
        this.email = email;
        this.address = address;
        this.name = name;
        this.phone = phone;
        this.shortBio = shortBio;
    }
}
