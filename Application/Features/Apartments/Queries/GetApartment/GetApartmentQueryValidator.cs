using FluentValidation;

namespace Application.Features.Apartments.Queries.GetApartment
{
    public class GetApartmentQueryValidator : AbstractValidator<GetApartmentQuery>
    {
        public GetApartmentQueryValidator()
            => RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty()
                .NotEqual(Guid.Empty);
    }
}
