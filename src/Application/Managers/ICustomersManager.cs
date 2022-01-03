using System;
using System.Threading;
using System.Threading.Tasks;

namespace RocketStore.Application.Managers
{
    /// <summary>
    /// Defines the interface of the customers manager.
    /// The customers manager allows retrieving, creating, and deleting customers.
    /// </summary>
    public interface ICustomersManager
    {
        #region Methods

        /// <summary>
        /// Creates the specified customer.
        /// </summary>
        /// <param name="customer2">The customer that should be created.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The <see cref="Task{TResult}" /> that represents the asynchronous operation.
        /// The <see cref="Result{T}" /> that describes the result.
        /// The new customer identifier.
        /// </returns>
        Task<Result<Guid>> CreateCustomerAsync(Models.Customer2 customer2, CancellationToken cancellationToken = default);

        #endregion
    }
}
