namespace RocketStore.Application.Customer.Queries.GetCustomerDetail
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using RocketStore.Application.Common.Exceptions;
    using RocketStore.Application.Common.Interfaces;
    using RocketStore.Domain.Entities;

    public class GetCustomerDetailQueryHandler : IRequestHandler<GetCustomerDetailQuery, CustomerDetailDto>
    {
        private readonly IApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public GetCustomerDetailQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
        }
        
        public async Task<CustomerDetailDto> Handle(GetCustomerDetailQuery request, CancellationToken cancellationToken)
        {
            request = request ?? throw new ArgumentNullException(nameof(request));
            
            var customer = await applicationDbContext.Customers
                .ProjectTo<CustomerDetailDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }

            return customer;
        }
    }
}