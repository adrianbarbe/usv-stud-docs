using FluentValidation;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Validators
{
    public class FacultyValidator : AbstractValidator<Faculty>
    {
        public FacultyValidator()
        {
            RuleFor(f => f.Name)
                .NotEmpty()
                .WithMessage("Field should not be empty");
            
            RuleFor(f => f.NameShort)
                .NotEmpty()
                .WithMessage("Field should not be empty");

            RuleFor(f => f.OrderBy)
                .NotEmpty()
                .WithMessage("Field should not be empty");
            
            RuleFor(f => f.Dean)
                .NotNull()
                .WithMessage("Field should not be empty");
            
            RuleFor(f => f.SecretaryHead)
                .NotNull()
                .WithMessage("Field should not be empty");
        }
    }
}