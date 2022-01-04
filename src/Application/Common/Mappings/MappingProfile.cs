namespace RocketStore.Application.Common.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using RocketStore.Application.Customer.Queries.GetCustomerDetail;
    using RocketStore.Application.Customer.Queries.GetCustomerList;
    using Domain = Domain.Entities;

    /// <summary>
    /// Defines the mapping profile used by the application.
    /// </summary>
    /// <seealso cref="Profile" />
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Created via dependency injection.")]
    public class MappingProfile : Profile
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            this.CreateMap<Domain.Customer, CustomersDto>();
            this.CreateMap<Domain.Customer, CustomerDetailDto>()
                .ForMember(dto => dto.Address, expression => expression.Ignore());
            this.CreateMap<Domain.AddressData, AddressDataDto>()
                .ForMember(dto => dto.CountryCode, expression => expression.MapFrom(data => data.Country_Code))
                .ForMember(dto => dto.PostalCode, expression => expression.MapFrom(data => data.Postal_Code))
                .ForMember(dto => dto.RegionCode, expression => expression.MapFrom(data => data.Region_Code));
        }

        #endregion
    }
}
