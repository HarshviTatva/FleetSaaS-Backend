using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Common.Messages;
using FluentValidation;

namespace FleetSaaS.Application.Validators
{
    public class AssignVehicleRequestValidator : AbstractValidator<AssignVehicleRequest>
    {
        public AssignVehicleRequestValidator() 
        {
            RuleFor(x => x.VehicleId)
                .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED);

            RuleFor(x => x.DriverId)
               .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED);
        }
    }
}
