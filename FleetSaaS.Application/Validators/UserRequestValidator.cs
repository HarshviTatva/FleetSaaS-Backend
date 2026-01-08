using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common;
using FluentValidation;

namespace FleetSaaS.Application.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest> 
    {
        public UserRequestValidator() 
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
        }
    }
}
