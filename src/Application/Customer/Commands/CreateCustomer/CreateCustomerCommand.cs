namespace RocketStore.Application.Customer.Commands.CreateCustomer
{
    using MediatR;
    
    public class CreateCustomerCommand : IRequest<string>
    {
        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the customer email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the customer VAT number.
        /// </summary>
        public string VatNumber { get; set; }
    }
}