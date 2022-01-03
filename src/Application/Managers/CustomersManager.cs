namespace RocketStore.Application.Managers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using RocketStore.Application.Common.Interfaces;
    using Entities = Domain.Entities;

    /// <summary>
    /// Defines the default implementation of <see cref="ICustomersManager"/>.
    /// </summary>
    /// <seealso cref="ICustomersManager" />
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Created via dependency injection.")]
    public class CustomersManager : ICustomersManager
    {
        #region Private Properties

        private IApplicationDbContext Context
        {
            get;
        }

        private IMapper Mapper
        {
            get;
        }

        private ILogger Logger
        {
            get;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersManager" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public CustomersManager(IApplicationDbContext context, IMapper mapper, ILogger<CustomersManager> logger)
        {
            this.Context = context;
            this.Mapper = mapper;
            this.Logger = logger;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public async Task<Result<Guid>> CreateCustomerAsync(Models.Customer2 customer2, CancellationToken cancellationToken = default)
        {
            customer2 = customer2 ?? throw new ArgumentNullException(nameof(customer2));

            Entities.Customer entity = this.Mapper.Map<Models.Customer2, Entities.Customer>(customer2);

            if (this.Context.Customers.Any(i => i.Email == entity.Email))
            {
                this.Logger.LogWarning($"A customer with email '{entity.Email}' already exists.");

                return Result<Guid>.Failure(
                    ErrorCodes.CustomerAlreadyExists,
                    $"A customer with email '{entity.Email}' already exists.");
            }

            this.Context.Customers.Add(entity);

            await this.Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            this.Logger.LogInformation($"Customer '{customer2.Name}' created successfully.");

            return Result<Guid>.Success(
                new Guid(entity.Id));
        }

        #endregion
    }
}
