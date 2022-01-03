namespace RocketStore.Infrastructure.Storage
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;
    using RocketStore.Application.Common.Interfaces;
    using RocketStore.Domain.Entities;

    /// <summary>
    /// Defines the application database context.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Created via dependency injection.")]
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the customers database set.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ApplicationDbContext(DbContextOptions context)
            : base(context)
        {
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));
            
            base.OnModelCreating(modelBuilder);

            // Customers

            modelBuilder.Entity<Customer>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Customer>().Property(t => t.Name).HasMaxLength(200).IsRequired();
            modelBuilder.Entity<Customer>().Property(t => t.Email).HasMaxLength(200).IsRequired();
            modelBuilder.Entity<Customer>().Property(t => t.VatNumber).HasMaxLength(9);
            modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();
            modelBuilder.Entity<Customer>().HasKey(customer => customer.Id);
        }

        #endregion
    }
}
