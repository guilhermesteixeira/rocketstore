namespace RocketStore.Application.Customer.Commands.CreateCustomer
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using RocketStore.Application.Common.Exceptions;
    using RocketStore.Application.Common.Interfaces;
    using RocketStore.Application.Managers;
    using RocketStore.Application.Models;
    using RocketStore.Domain.Entities;

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, string>
    {
        private readonly IApplicationDbContext applicationDbContext;
        private readonly ILogger<CreateCustomerCommandHandler> logger;

        public CreateCustomerCommandHandler(IApplicationDbContext applicationDbContext, ILogger<CreateCustomerCommandHandler> logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }
        
        public async Task<string> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            var entity = new Customer
            {
                Name = request.Name,
                Email = request.EmailAddress,
                VatNumber = request.VatNumber
            };

            if (this.applicationDbContext.Customers.Any(i => i.Email == entity.Email))
            {
                this.logger.LogWarning($"A customer with email '{entity.Email}' already exists.");

                throw new CustomerAlreadyExistsException(entity.Email);
            }

            this.applicationDbContext.Customers.Add(entity);

            await this.applicationDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            this.logger.LogInformation($"Customer '{entity.Name}' created successfully.");

            return entity.Id;
        }
    }
}