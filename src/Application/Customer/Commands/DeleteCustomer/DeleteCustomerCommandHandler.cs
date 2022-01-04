namespace RocketStore.Application.Customer.Commands.DeleteCustomer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using RocketStore.Application.Common.Exceptions;
    using RocketStore.Application.Common.Interfaces;

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly IApplicationDbContext applicationDbContext;

        public DeleteCustomerCommandHandler(IApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        
        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            request = request ?? throw new ArgumentNullException(nameof(request));
            
            var customer = await applicationDbContext.Customers
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }

            applicationDbContext.Customers.Remove(customer);
            await applicationDbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}