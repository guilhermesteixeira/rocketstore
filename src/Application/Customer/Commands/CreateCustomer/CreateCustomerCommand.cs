namespace RocketStore.Application.Customer.Commands.CreateCustomer
{
    using System;
    using MediatR;
    
    public class CreateCustomerCommand : IRequest<Guid>
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
        
        /// <summary>
        /// Gets or sets the customer Address.
        /// </summary>
        public string Address
        {
            get;
            set;
        }
    }
}