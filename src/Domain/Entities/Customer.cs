namespace RocketStore.Domain.Entities
{
    using System;

    /// <summary>
    /// Defines a customer.
    /// </summary>
    public  class Customer
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
