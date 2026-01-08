using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common;
using FluentValidation;

namespace FleetSaaS.Application.Validators
{
    public class DriverUserRequestValidator : AbstractValidator<DriverUserRequest>
    {
        public DriverUserRequestValidator() 
        {
            RuleFor(x => x.UserName)
              .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED);

            RuleFor(x => x.Email)
              .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
              .EmailAddress().WithMessage(ValidationMessages.EMAIL_INVALID_FORMAT);

            RuleFor(x => x.PhoneNumber)
              .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
              .Matches(Fields.PHONE_REGEX)
              .WithMessage(ValidationMessages.PHONE_NUMBER_FORMAT);

            RuleFor(x => x.LicenseNumber)
              .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
              .Matches(Fields.LICENSE_REGEX)
              .WithMessage(ValidationMessages.LICENSE_NUMBER_FORMAT);

            RuleFor(x => x.LicenseExpiryDate)
              .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED);
        }
    }
}
