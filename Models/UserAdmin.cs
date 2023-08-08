namespace web.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class UserAdmin
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string UserID { get; set; }

    public string name { get; set; }
    public string? lastName { get; set; }
    public string? password { get; set; }
    public string phone { get; set; }
    public string email { get; set; }
    public Boolean isSuper { get; set; }
    public string role = "admin";

    public UserAdmin(
        string id,
        string lastName,
        string password,
        string name,
        string userID,
        string phone,
        string email,
        Boolean isSuper
    )
    {
        this.Id = id;
        this.password = password;
        this.lastName = lastName;
        this.email = email;
        this.UserID = userID;
        this.name = name;
        this.phone = phone;
        this.isSuper = isSuper;
    }

    public UserAdmin() { }
}
