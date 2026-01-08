using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common;
using FluentValidation;

namespace FleetSaaS.Application.Validators
{
    public class VehicleRequestValidator : AbstractValidator<VehicleRequest>
    {
        public VehicleRequestValidator() 
        {
            RuleFor(x => x.Make)
                .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
                .MaximumLength(15).WithMessage(ValidationMessages.VEHICLE_MAX_LENGTH);

            RuleFor(x => x.Model)
               .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
               .MaximumLength(15).WithMessage(ValidationMessages.VEHICLE_MAX_LENGTH);

            RuleFor(x => x.Year)
               .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED);

            RuleFor(x => x.Vin)
               .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
               .Matches(Fields.VIN_REGEX).WithMessage(ValidationMessages.VIN_FORMAT);

            RuleFor(x => x.InsuranceExpiryDate)
                .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED);

            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage(ValidationMessages.FIELD_REQUIRED)
                .Matches(Fields.LICENSE_PLATE_REGEX).WithMessage(ValidationMessages.LICENSE_PLATE_FORMAT);
        }
    }
}
