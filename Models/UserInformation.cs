namespace web.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class UserInformation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string name { get; set; }
    public string address { get; set; }
    public string profession { get; set; }
    public string? lastName { get; set; }
    public string? password { get; set; }
    public string phone { get; set; }
    public string email { get; set; }
    public string shortBio { get; set; }
    public string role = "user";

    public UserInformation(
        string id,
        string profession,
        string lastName,
        string password,
        string name,
        string shortBio,
        string phone,
        string email,
        string address
    )
    {
        this.profession = profession;
        this.Id = id;
        this.password = password;
        this.lastName = lastName;
        this.email = email;
        this.address = address;
        this.name = name;
        this.phone = phone;
        this.shortBio = shortBio;
    }
}
