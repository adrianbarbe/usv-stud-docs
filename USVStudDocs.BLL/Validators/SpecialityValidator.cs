using FluentValidation;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Validators
{
    public class SpecialityValidator : AbstractValidator<ProgramStudy>
    {
        public SpecialityValidator()
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

            RuleFor(f => f.Faculty)
                .NotEmpty()
                .WithMessage("Field should not be empty");

            RuleForEach(f => f.YearSemesters)
                .NotNull()
                .WithMessage("Semesters cannot be null")
                .SetValidator(new SemesterValidator());
        }
    }
}