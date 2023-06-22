using FluentValidation;
using USVStudDocs.Models.Student;

namespace USVStudDocs.BLL.Validators;

public class StudentCertificateCreateItemValidator : AbstractValidator<StudentCertificateCreateItem>
{
    public StudentCertificateCreateItemValidator()
    {
        RuleFor(f => f.Reason)
            .NotEmpty()
            .WithMessage("Reason should not be empty");
    }
}