namespace web.Models;

public class MongoDbSettings
{
    public string ConnectionUrl { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string collectionOne { get; set; } = null!;
    public string collectionTwo { get; set; } = null!;
    public string collectionThree { get; set; } = null!;
}
