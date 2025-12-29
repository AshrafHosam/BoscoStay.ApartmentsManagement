using FluentValidation;

namespace Application.Features.Apartments.Commands.CreateApartment
{
    public class CreateApartmentCommandValidator : AbstractValidator<CreateApartmentCommand>
    {
        public CreateApartmentCommandValidator()
        {
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
                .GreaterThanOrEqualTo(0.0);

        }
    }
}
