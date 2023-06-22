using FluentValidation;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Validators
{
    public class FacultyPersonValidator : AbstractValidator<FacultyPerson>
    {
        public FacultyPersonValidator()
        {
            RuleFor(f => f.Name)
                .NotEmpty()
                .WithMessage("Field should not be empty");
            
            RuleFor(f => f.Surname)
                .NotEmpty()
                .WithMessage("Field should not be empty");

            RuleFor(f => f.Patronymic)
                .NotEmpty()
                .WithMessage("Field should not be empty");
            
            RuleFor(f => f.PersonType)
                .NotNull()
                .WithMessage("Field should not be empty");
            
        }
    }
}