namespace RocketStore.Application.Customer.Queries.GetCustomerList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using RocketStore.Application.Common.Interfaces;

    public class GetCustomersListQueryHandler : IRequestHandler<GetCustomersListQuery, IList<CustomersDto>>
    {
        private readonly IApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public GetCustomersListQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
        }
        
        public async Task<IList<CustomersDto>> Handle(GetCustomersListQuery request, CancellationToken cancellationToken)
        {
            request = request ?? throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.filter))
            {
                return await applicationDbContext.Customers
                    .ProjectTo<CustomersDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }

            return await applicationDbContext.Customers.Where(x => x.Name.Contains(request.filter) || x.Email.Contains(request.filter) )
                .ProjectTo<CustomersDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}