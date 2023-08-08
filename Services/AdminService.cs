using web.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace web.Services;

public class AdminService
{
    private readonly IConfiguration _config;
    private readonly IMongoCollection<UserAdmin> _adminCollection;
    private ServiceProviderService _myService;
    private EmployerService _allEmployers;
    private List<UserAdmin> tmp;

    public AdminService(IOptions<MongoDbSettings> mongoDbSettings, ServiceProviderService myService, EmployerService employer)

    {
        _myService = myService;
        _allEmployers = employer;

        MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionUrl);
        IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _adminCollection = database.GetCollection<UserAdmin>(mongoDbSettings.Value.collectionTwo);

    }
    public async Task<bool> AddAdmin(UserAdmin UserInformation)
    {
        try
        {
            await _adminCollection.InsertOneAsync(UserInformation);

            return true;

        }
        catch (Exception)
        {
            return false;
        }
    }

    public Task<List<UserAdmin>> GetAllAdmins(string id)
    {



        tmp = _adminCollection.Find(_ => true).ToList();

        return Task.FromResult(tmp);


    }
    public async Task<List<Employer>> GetAllEmployers()
    {

        return await _allEmployers.GetAllEmployers();




    }
    public async Task<bool> UpdateAdmin(string id, AdminUpdate updateInfo)
    {



        try
        {
            FilterDefinition<UserAdmin> filter = Builders<UserAdmin>.Filter.Eq("Id", id);
            UpdateDefinition<UserAdmin> update = Builders<UserAdmin>.Update
            .Set("name", updateInfo.name)
            .Set("lastName", updateInfo.lastName)
            .Set("email", updateInfo.email)
            .Set("phone", updateInfo.phone);
            var result = _adminCollection.UpdateOne(filter, update);
            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            // handle the exception here
            return false;
        }
    }
    public async Task<bool> DeleteAdminAsync(string id)
    {
        try
        {
            FilterDefinition<UserAdmin> filter = Builders<UserAdmin>.Filter.Eq("Id", id);
            var result = _adminCollection.DeleteOne(filter);
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            // handle the exception here
            return false;
        }


    }
    public Task<UserAdmin> AdminLogIN(LoginModel loginInfo)
    {
        return Task.FromResult(_adminCollection.Find(t => t.name == loginInfo.name && t.password == loginInfo.password).FirstOrDefault());


    }
    public async Task<List<UserInformation>> GetServicProviders()
    {
        return await _myService.GetAsync();
    }


}