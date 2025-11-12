using FluentValidation;

namespace Application.Features.Apartments.Commands.DeleteApartment
{
    public class DeleteApartmentCommandValidator : AbstractValidator<DeleteApartmentCommand>
    {
        public DeleteApartmentCommandValidator()
            => RuleFor(a => a.Id)
                   .NotNull()
                   .NotEmpty()
                   .NotEqual(Guid.Empty);
    }
}
