using CustomerService.Models;
using CustomerService.V1.Models.RequestModels;
using CustomerService.V1.Models.ResponseModels;

namespace CustomerService.Mappers;

public class CustomerMapper : ICustomerMapper
{
    public Customer CreateModelToModel(CustomerCreateModel createModel)
    {    
        return new Customer
        {
            Name = createModel.Name,
            Email = createModel.Email,
            Addresses = createModel.Addresses?.Select(x => new Address
            {
                AddressLine = x.AddressLine,
                City = x.City,
                Country = x.Country,
                CityCode = x.CityCode
            }).ToList(),
            UpdatedAt = null
        };
    }

    public CustomerGetModel ModelToGetModel(Customer customer)
    {
        return new CustomerGetModel
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Addresses = customer.Addresses?.Select(x => new CustomerGetModel.AddressGetModel
            {
                AddressLine = x.AddressLine,
                City = x.City,
                Country = x.Country,
                CityCode = x.CityCode
            }).ToList(),
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }

    public IEnumerable<CustomerGetModel> ModelToGetAllModel(IEnumerable<Customer> customers)
    {
        return customers.Select(c => new CustomerGetModel
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Addresses = c.Addresses?.Select(a => new CustomerGetModel.AddressGetModel
            {
                AddressLine = a.AddressLine,
                City = a.City,
                Country = a.Country,
                CityCode = a.CityCode
            }).ToList(),
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        });
    }

    public Customer PatchModelToModel(CustomerPatchModel patchModel, Customer oldCustomer)
    {
        
        oldCustomer.Name = patchModel.Name ?? oldCustomer.Name;
        oldCustomer.Email = patchModel.Email ?? oldCustomer.Email;
        
        for (int i = 0; i < oldCustomer.Addresses.Count; i++)
        {
            oldCustomer.Addresses[i].AddressLine = patchModel.Addresses[i].AddressLine ?? oldCustomer.Addresses[i].AddressLine;
            oldCustomer.Addresses[i].Country = patchModel.Addresses[i].Country ?? oldCustomer.Addresses[i].Country;
            oldCustomer.Addresses[i].CityCode = patchModel.Addresses[i].CityCode ?? oldCustomer.Addresses[i].CityCode;
            oldCustomer.Addresses[i].City = patchModel.Addresses[i].City ?? oldCustomer.Addresses[i].City;
        }

        return oldCustomer;
    }
}