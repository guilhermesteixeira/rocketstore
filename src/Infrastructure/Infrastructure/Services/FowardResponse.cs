namespace RocketStore.Infrastructure.Services
{
    using System.Collections.Generic;
    using RocketStore.Domain.Entities;

    public class ForwardResponse
    {
        public IList<AddressData> Data { get; set; }
    }
}