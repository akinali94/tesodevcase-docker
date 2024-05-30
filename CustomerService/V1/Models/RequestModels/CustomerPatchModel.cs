namespace CustomerService.V1.Models.RequestModels;

public class CustomerPatchModel
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<AddressPatchModel>? Addresses { get; set; }

    public class AddressPatchModel
    {
        public string? AddressLine { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public int? CityCode { get; set; }
    }

}