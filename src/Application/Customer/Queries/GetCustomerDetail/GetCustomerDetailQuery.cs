namespace RocketStore.Application.Customer.Queries.GetCustomerDetail
{
    using System;
    using MediatR;

    public class GetCustomerDetailQuery : IRequest<CustomerDetailDto>
    {
        public Guid Id { get; set; }
    }
}