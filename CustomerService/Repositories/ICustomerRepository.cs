using CustomerService.Models;

namespace CustomerService.Repositories;

public interface ICustomerRepository
{
     Task<string> CreateAsync(Customer newCustomer);
     
    Task<bool> UpdateAsync(string id, Customer updatedCustomer);
    
    Task<bool> DeleteAsync(string id);

    Task<Customer> GetByIdAsync(string id);

    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer> GetByEmailAsync(string email);
}