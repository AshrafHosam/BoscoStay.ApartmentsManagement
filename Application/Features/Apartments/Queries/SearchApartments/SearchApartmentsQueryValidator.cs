using FluentValidation;

namespace Application.Features.Apartments.Queries.SearchApartments
{
    public class SearchApartmentsQueryValidator : AbstractValidator<SearchApartmentsQuery>
    {
        public SearchApartmentsQueryValidator()
        {
            RuleFor(a => a.SearchText)
                .MaximumLength(75);

            RuleFor(a => a.PageNumber)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(a => a.PageSize)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);

            When(a => a.MaxPrice.HasValue, () =>
            {
                RuleFor(a => a.MaxPrice)
                .GreaterThanOrEqualTo(0);
            });

            When(a => a.NoiseLevel.HasValue, () =>
            {
                RuleFor(a => a.NoiseLevel)
                .IsInEnum();
            });

            When(a => a.MaxArea.HasValue, () =>
            {
                RuleFor(a => a.MaxArea)
                .GreaterThan(0);
            });

            When(a => a.MaxDistanceToCenter.HasValue, () =>
            {
                RuleFor(a => a.MaxDistanceToCenter)
                .GreaterThan(0);
            });
        }
    }
}
