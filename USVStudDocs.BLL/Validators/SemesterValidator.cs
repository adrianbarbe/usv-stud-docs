using FluentValidation;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Validators
{
    public class SemesterValidator : AbstractValidator<YearSemester>
    {
        public SemesterValidator()
        {
            RuleFor(s => s.YearNumber)
                .NotEmpty()
                .WithMessage("Year number cannot be empty");
        }
    }
}