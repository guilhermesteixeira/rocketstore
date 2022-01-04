namespace RocketStore.Application.Customer.Queries.GetCustomerDetail
{
    using System;

    /// <summary>
    /// Defines a customer.
    /// </summary>
    public  class CustomerDetailDto
    {
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the customer email address.
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the customer VAT number.
        /// </summary>
        public string VatNumber
        {
            get;
            set;
        }
    }
}