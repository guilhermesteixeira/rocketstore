namespace RocketStore.Application.Customer.Queries.GetCustomerDetail
{
    public class AddressDataDto
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public string RegionCode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string MapUrl { get; set; }
    }
}