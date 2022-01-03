namespace RocketStoreApi.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using RocketStore.Application.Customer.Commands.CreateCustomer;

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

            /*
            if (result.FailedWith(ErrorCodes.CustomerAlreadyExists))
            {
                return this.Conflict(
                    new ProblemDetails()
                    {
                        Status = (int)HttpStatusCode.Conflict,
                        Title = result.ErrorCode,
                        Detail = result.ErrorDescription,
                    });
            }
            else if (result.Failed)
            {
                return this.BadRequest(
                    new ProblemDetails()
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Title = result.ErrorCode,
                        Detail = result.ErrorDescription,
                    });
            }*/

            return this.Created(
                this.GetUri("customers", result),
                Guid.Parse(result));
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
