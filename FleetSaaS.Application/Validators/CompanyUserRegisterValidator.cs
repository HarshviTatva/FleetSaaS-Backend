using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.Interfaces.IRepositories;
using FluentValidation;
namespace FleetSaaS.Application.Validators
{
    public class CompanyUserRegisterValidator : AbstractValidator<CompanyUserRegisterRequest>
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyUserRegisterValidator(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company Name is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d{10,13}$")
                .WithMessage("Phone number must be 10 to 13 digits.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.")
                .MustAsync(BeUniqueCompanyEmail)
                .WithMessage("Company email already exists.");

            RuleFor(x => x.OwnerName)
                .NotEmpty().WithMessage("Owner Name is required.");

            RuleFor(x => x.OwnerPhoneNumber)
                .NotEmpty().WithMessage("Owner phone number is required.")
                .Matches(@"^\+?\d{10,13}$")
                .WithMessage("Owner phone number must be 10 to 13 digits.");

            RuleFor(x => x.OwnerEmail)
                .NotEmpty().WithMessage("Owner Email is required.")
                .EmailAddress().WithMessage("Invalid owner email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters.");
        }

        private async Task<bool> BeUniqueCompanyEmail(
            string email,
            CancellationToken token)
        {
            return !await _companyRepository.ExistsByEmailAsync(email);
        }
    }
}
