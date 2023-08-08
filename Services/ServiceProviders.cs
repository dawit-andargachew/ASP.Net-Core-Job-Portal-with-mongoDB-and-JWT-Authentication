using web.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace web.Services;
public class ServiceProviderService
{
    private readonly IMongoCollection<UserInformation> _userCollection;
    public ServiceProviderService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionUrl);
        IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _userCollection = database.GetCollection<UserInformation>(mongoDbSettings.Value.collectionOne);

    }

    public ServiceProviderService()
    {
    }

    public async Task<bool> CreateAsync(UserInformation UserInformation)
    {
        try
        {
            await _userCollection.InsertOneAsync(UserInformation);
            return true;

        }
        catch (Exception)
        {
            return false;
        }


    }

    public async Task<List<UserInformation>> GetAsync()
    {
        return await _userCollection.Find(_ => true).ToListAsync();
    }
    public UserInformation GetProfile(string id)
    {
        return _userCollection.Find(t => t.Id == id).FirstOrDefault();
    }
    public bool UpdateUsers(string id, UpdateModel updateInfo)
    {

        try
        {
            var filter = Builders<UserInformation>.Filter.Eq("Id", ObjectId.Parse(id));
            UpdateDefinition<UserInformation> update = Builders<UserInformation>.Update.Set("name", updateInfo.name)
            .Set("shortBio", updateInfo.shortBio)
            .Set("lastName", updateInfo.lastName)
            .Set("email", updateInfo.email)
            .Set("address", updateInfo.address)
            .Set("profession", updateInfo.profession)
            .Set("phone", updateInfo.phone);
            var result = _userCollection.UpdateOne(filter, update);
            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            // handle the exc
            Console.WriteLine(ex);

            return false;
        }
    }
    public bool Delete(string id)
    {
        try
        {
            FilterDefinition<UserInformation> filter = Builders<UserInformation>.Filter.Eq("Id", id);
            var result = _userCollection.DeleteOne(filter);
            return result.DeletedCount > 0;


        }
        catch (Exception ex)
        {
            // handle the exception here
            return false;
        }





    }
    public Task<UserInformation> ServiceProviderLogin(LoginModel loginInfo)
    {
        return Task.FromResult(_userCollection.Find(t => t.name == loginInfo.name && t.password == loginInfo.password).FirstOrDefault());


    }




}
