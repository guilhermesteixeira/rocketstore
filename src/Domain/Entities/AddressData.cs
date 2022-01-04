namespace RocketStore.Domain.Entities
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Resharper", "CA1707", Justification = "External contract.")]
    public class AddressData
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public string Street { get; set; }
        public string Postal_Code { get; set; }
        public string Region { get; set; }
        public string Region_Code { get; set; }
        public string Country { get; set; }
        public string Country_Code { get; set; }
        public string Map_Url { get; set; }
    }
}