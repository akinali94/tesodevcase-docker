namespace CustomerService.V1.Models.RequestModels;

public class CustomerCreateModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public List<AddressModel> Addresses { get; set; }

    public class AddressModel
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CityCode { get; set; }
    }
}