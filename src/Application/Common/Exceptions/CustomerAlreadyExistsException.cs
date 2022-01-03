namespace RocketStore.Application.Common.Exceptions
{
    using System;

    public class CustomerAlreadyExistsException : Exception
    {
        public CustomerAlreadyExistsException() : base()
        {
            
        }

        public CustomerAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
        
        public CustomerAlreadyExistsException(string email)
            : base($"A customer with email '{email}' already exists.")
        {
            
        }
    }
}