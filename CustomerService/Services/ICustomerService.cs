using CustomerService.Models;
using CustomerService.V1.Models.RequestModels;
using CustomerService.V1.Models.ResponseModels;

namespace CustomerService.Services;

public interface ICustomerService
{
    Task<string> Create(CustomerCreateModel newCustomer);
    Task<bool> Update(string id, CustomerPatchModel updatedCustomer);
    Task<bool> Delete(string id);
    Task<CustomerGetModel> GetById(string id);
    Task<IEnumerable<CustomerGetModel>> GetAll();
    Task<bool> Validate(string id);
}