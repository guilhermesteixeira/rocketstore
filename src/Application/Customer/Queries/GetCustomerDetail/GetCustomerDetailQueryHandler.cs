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
        private readonly IPositionService positionService;

        public GetCustomerDetailQueryHandler(IApplicationDbContext applicationDbContext,
            IMapper mapper,
            IPositionService positionService)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.positionService = positionService;
        }

        public async Task<CustomerDetailDto> Handle(GetCustomerDetailQuery request, CancellationToken cancellationToken)
        {
            request = request ?? throw new ArgumentNullException(nameof(request));
            
            var customer = await applicationDbContext.Customers
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }
            
            var customerDto = mapper.Map<Customer, CustomerDetailDto>(customer);

            var addressData = await positionService.GetAddress(customer.Address);

            if (addressData != null)
            {
                customerDto.Address = mapper.Map<AddressData, AddressDataDto>(addressData);
            }

            return customerDto;
        }
    }
}