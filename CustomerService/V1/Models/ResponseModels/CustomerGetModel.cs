namespace CustomerService.V1.Models.ResponseModels;

public class CustomerGetModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<AddressGetModel> Addresses { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public class AddressGetModel
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CityCode { get; set; }
    }
}