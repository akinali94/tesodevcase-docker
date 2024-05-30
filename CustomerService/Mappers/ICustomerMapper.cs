using CustomerService.Models;
using CustomerService.V1.Models.RequestModels;
using CustomerService.V1.Models.ResponseModels;

namespace CustomerService.Mappers;

public interface ICustomerMapper
{
    Customer CreateModelToModel(CustomerCreateModel createModel);
    CustomerGetModel ModelToGetModel(Customer customer);
    IEnumerable<CustomerGetModel> ModelToGetAllModel(IEnumerable<Customer> customers);
    Customer PatchModelToModel(CustomerPatchModel patchModel, Customer oldCustomer);
}