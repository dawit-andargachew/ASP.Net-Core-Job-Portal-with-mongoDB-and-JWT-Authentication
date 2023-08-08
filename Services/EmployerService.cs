using web.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace web.Services;

public class EmployerService
{
    private readonly IMongoCollection<Employer> _employerCollection;
    public EmployerService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionUrl);
        IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _employerCollection = database.GetCollection<Employer>(mongoDbSettings.Value.collectionThree);

    }

    public EmployerService()
    {
    }

    public async Task<bool> RegisterEmployer(Employer employerInfo)
    {
        try
        {
            await _employerCollection.InsertOneAsync(employerInfo);
            return true;

        }
        catch (Exception)
        {
            return false;
        }


    }

    public async Task<List<Employer>> GetAllEmployers()
    {
        return await _employerCollection.Find(_ => true).ToListAsync();
    }
    public async Task<Employer> Getprofile(string id)
    {
        return _employerCollection.Find(t => t.Id == id).FirstOrDefault();
    }
    public async Task<bool> UpdateEmployer(string id, EmployerUpdate updateInfo)
    {
        try
        {
            FilterDefinition<Employer> filter = Builders<Employer>.Filter.Eq("Id", id);
             UpdateDefinition<Employer> update = Builders<Employer>.Update
            .Set("name", updateInfo.name)
            .Set("lastName", updateInfo.lastName)
            .Set("email", updateInfo.email)
            .Set("phone", updateInfo.phone);
            var result = _employerCollection.UpdateOne(filter,update);
            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            // handle the exception here
            return false;
        }





    }
    public bool DeleteEmployersync(string id)
    {
        try
        {
            FilterDefinition<Employer> filter = Builders<Employer>.Filter.Eq("Id", id);
            var result = _employerCollection.DeleteOne(filter);
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            // handle the exception here
            return false;
        }




    }
    public Task<Employer> EmployerLogin(LoginModel loginInfo)
    {
        return Task.FromResult(_employerCollection.Find(t => t.name == loginInfo.name && t.password == loginInfo.password).FirstOrDefault());


    }


}