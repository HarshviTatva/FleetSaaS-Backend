using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Common.Messages;
using FluentValidation;

namespace FleetSaaS.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
       public LoginRequestValidator() 
       {
          RuleFor(x => x.Email)
          .NotEmpty().WithMessage(ValidationMessages.EMAIL_REQUIRED)
          .EmailAddress().WithMessage(ValidationMessages.EMAIL_INVALID_FORMAT);

          RuleFor(x => x.Password)
              .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
              .MinimumLength(8)
              .WithMessage(ValidationMessages.PASSWORD_FORMAT);

        }
    }
}
