namespace RocketStore.Application.Customer.Queries.GetCustomerList
{
    using System.Collections.Generic;
    using MediatR;

    public class GetCustomersListQuery : IRequest<IList<CustomersDto>>
    {
        public string? filter { get; set; }
    }
}