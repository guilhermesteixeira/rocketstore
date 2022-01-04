namespace RocketStore.Application.Common.Mappings
{
    using System;
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
            this.CreateMap<Models.Customer2, Domain.Customer>()
                .ForMember(customer => customer.Email,
                    expression => expression.MapFrom(customer => customer.EmailAddress))
                .AfterMap(
                    (source, target) =>
                    {
                        target.Id = Guid.NewGuid();
                    });

            this.CreateMap<Domain.Customer, CustomersDto>();
            this.CreateMap<Domain.Customer, CustomerDetailDto>();
        }

        #endregion
    }
}
