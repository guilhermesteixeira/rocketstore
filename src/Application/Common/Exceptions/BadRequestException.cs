namespace RocketStore.Application.Common.Exceptions
{
    using System;

    public class BadRequestException : Exception
    {
        public BadRequestException() : base()
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}