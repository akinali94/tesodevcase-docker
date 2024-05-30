using CustomerService.Helpers;
using CustomerService.Mappers;
using CustomerService.Models;
using CustomerService.Repositories;
using CustomerService.V1.Models.RequestModels;
using CustomerService.V1.Models.ResponseModels;

namespace CustomerService.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly ICustomerMapper _mapper;

    public CustomerService(ICustomerRepository repository, ICustomerMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<string> Create(CustomerCreateModel createdModel)
    {
        if (await _repository.GetByEmailAsync(createdModel.Email) != null)
            throw new CustomException("This Email already registered");
        
        Customer newCustomer = _mapper.CreateModelToModel(createdModel);
        
        newCustomer.Id = Guid.NewGuid().ToString();
        newCustomer.CreatedAt = DateTime.Now;
        
        return await _repository.CreateAsync(newCustomer);
    }

    public async Task<bool> Update(string id, CustomerPatchModel updatedModel)
    {
        Customer oldCustomer = await _repository.GetByIdAsync(id);
        if(oldCustomer is null)
            throw new KeyNotFoundException("Customer not found on Update Service");

        oldCustomer = _mapper.PatchModelToModel(updatedModel, oldCustomer);
        oldCustomer.UpdatedAt = DateTime.Now;
        
        return await _repository.UpdateAsync(id, oldCustomer);

    }

    public async Task<bool> Delete(string id)
    {
        if (await _repository.GetByIdAsync(id) is null)
            throw new KeyNotFoundException("Customer not found on Delete Service");

        return await _repository.DeleteAsync(id);
    }

    public async Task<CustomerGetModel> GetById(string id)
    {
        var customer = await _repository.GetByIdAsync(id);
        
        if (customer == null)
            throw new KeyNotFoundException("No Customer Found with this Id");
        
        return _mapper.ModelToGetModel(customer);
    }

    public async Task<IEnumerable<CustomerGetModel>> GetAll()
    {
         var customers = await _repository.GetAllAsync();
         
         if (customers == null)
             throw new KeyNotFoundException("No Customers Found");
    
         return _mapper.ModelToGetAllModel(customers);
    }

    public async Task<bool> Validate(string id)
    {
        var customer = await _repository.GetByIdAsync(id);
        
        if (customer is null)
            return false;
        
        return true;
    }
}