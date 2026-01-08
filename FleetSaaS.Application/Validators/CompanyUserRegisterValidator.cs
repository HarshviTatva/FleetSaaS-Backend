using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common;
using FluentValidation;

namespace FleetSaaS.Application.Validators
{
    public class CompanyUserRegisterValidator : AbstractValidator<CompanyUserRegisterRequest>
    {
        public CompanyUserRegisterValidator()
        {

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
                .Matches(Fields.PHONE_REGEX)
                .WithMessage(ValidationMessages.PHONE_NUMBER_FORMAT);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
                .EmailAddress().WithMessage(ValidationMessages.EMAIL_INVALID_FORMAT);

            RuleFor(x => x.OwnerName)
                .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED);

            RuleFor(x => x.OwnerPhoneNumber)
                .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
                .Matches(Fields.PHONE_REGEX)
                .WithMessage(ValidationMessages.PHONE_NUMBER_FORMAT);

            RuleFor(x => x.OwnerEmail)
                .NotEmpty().WithMessage(ValidationMessages.EMAIL_REQUIRED)
                .EmailAddress().WithMessage(ValidationMessages.EMAIL_INVALID_FORMAT);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
                .MinimumLength(8)
                .WithMessage(ValidationMessages.PASSWORD_FORMAT);
        }
    }
}
