using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Common.Messages;
using FluentValidation;

namespace FleetSaaS.Application.Validators
{
    public class TripRequestValidator : AbstractValidator<TripRequest>
    {
        public TripRequestValidator() 
        {
            RuleFor(x => x.Origin)
             .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
             .MaximumLength(25).WithMessage(ValidationMessages.TRIP_MAX_LENGTH);

            RuleFor(x => x.Description)
               .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
               .MaximumLength(25).WithMessage(ValidationMessages.TRIP_MAX_LENGTH);

            RuleFor(x => x.Destination)
               .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
               .MaximumLength(100).WithMessage(ValidationMessages.DESCRIPTION_MAX_LENGTH);

        }
    }
}
