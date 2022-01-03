namespace RocketStore.Application.Common.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RocketStore.Domain.Entities;

    public interface IApplicationDbContext
    {
        /// <summary>
        /// Gets or sets the customers database set.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}