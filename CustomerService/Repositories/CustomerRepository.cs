using CustomerService.Configs;
using CustomerService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CustomerService.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IMongoCollection<Customer> _collection;

    public CustomerRepository(IOptions<CustomerDbSettings> customerDbSettings)
    {
        var mongoClient = new MongoClient(customerDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(customerDbSettings.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<Customer>(
            customerDbSettings.Value.CustomersCollectionName);
    }
    
    public async Task<string> CreateAsync(Customer newCustomer)
    {
        await _collection.InsertOneAsync(newCustomer);
        
        return newCustomer.Id;
    }
    
    public async Task<bool> UpdateAsync(string id, Customer updatedCustomer)
    {
        var result = await _collection.ReplaceOneAsync(x => x.Id == id, updatedCustomer);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }
    
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id);
        
        return result.IsAcknowledged;
    }

    public async Task<Customer> GetByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    }
    
    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _collection.Find(x => true).ToListAsync();
    }

    public async Task<Customer> GetByEmailAsync(string email)
    {
        var filter = Builders<Customer>.Filter.Eq(customer => customer.Email, email);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
