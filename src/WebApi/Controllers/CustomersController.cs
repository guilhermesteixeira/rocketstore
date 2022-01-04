namespace RocketStoreApi.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using RocketStore.Application.Customer.Commands.CreateCustomer;
    using RocketStore.Application.Customer.Queries.GetCustomerDetail;
    using RocketStore.Application.Customer.Queries.GetCustomerList;

    /// <summary>
    /// Defines the customers controller.
    /// This controller provides actions on customers.
    /// </summary>
    [ControllerName("Customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator mediator;

        // Ignore Spelling: api

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="mediator">Cqrs Mediator. </param>
        public CustomersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Creates the specified customer.
        /// </summary>
        /// <param name="customer">The customer that should be created.</param>
        /// <returns>
        /// The new customer identifier.
        /// </returns>
        [HttpPost("api/customers")]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateCustomerAsync(CreateCustomerCommand customer)
        {
            var result = await this.mediator.Send(customer).ConfigureAwait(false);

            return this.Created(
                this.GetUri("customers", result),
                result);
        }

        /// <summary>
        /// Get all customers by filter.
        /// </summary>
        /// <param name="filter">filter.</param>
        /// <returns>List of customers.</returns>
        [HttpGet("api/customers")]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(IList<CustomersDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCustomerAsync([FromQuery]string filter)
        {
            return this.Ok(await this.mediator.Send(new GetCustomersListQuery
                {
                    filter = filter,
                })
                .ConfigureAwait(false));
        }

        /// <summary>
        /// Get customer by Id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Customer.</returns>
        [HttpGet("api/customers/{id}")]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(CustomerDetailDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerByIdAsync([FromRoute]Guid id)
        {
            return this.Ok(await this.mediator.Send(new GetCustomerDetailQuery
                {
                    Id = id,
                })
                .ConfigureAwait(false));
        }

        private Uri GetUri(params object[] parameters)
        {
            string result = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            foreach (object pathParam in parameters)
            {
                result = $"{result}/{pathParam}";
            }

            return new Uri(result);
        }
    }
}
