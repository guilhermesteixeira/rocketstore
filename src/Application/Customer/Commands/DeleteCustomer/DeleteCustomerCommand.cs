namespace RocketStore.Application.Customer.Commands.DeleteCustomer
{
    using System;
    using MediatR;

    public class DeleteCustomerCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}