namespace RocketStore.Application.Customer.Commands.CreateCustomer
{
    using FluentValidation;

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("The Email field is not a valid e-mail address.");
            RuleFor(x => x.VatNumber).NotEmpty().Matches("^[0-9]{9}$");
            RuleFor(x => x.Address).NotEmpty();
        }
    }
}