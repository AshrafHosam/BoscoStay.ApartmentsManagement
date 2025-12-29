using FluentValidation;

namespace Application.Features.Apartments.Commands.UpdateApartment
{
    public class UpdateApartmentCommandValidator : AbstractValidator<UpdateApartmentCommand>
    {
        public UpdateApartmentCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(a => a.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(60);

            RuleFor(a => a.Description)
                .MaximumLength(350);

            RuleFor(a => a.Address)
                .NotNull()
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(a => a.Floor)
                .NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(a => a.NoiseLevel)
                .IsInEnum();

            RuleFor(a => a.DistanceToCenterInKm)
                .NotNull()
                .GreaterThanOrEqualTo(0.0);

            RuleFor(a => a.AreaInSquareMeters)
                .NotNull()
                .GreaterThanOrEqualTo(0.0);

            RuleFor(b => b.PricePerDay)
                .NotNull()
                .GreaterThanOrEqualTo(0.0);
        }
    }
}
