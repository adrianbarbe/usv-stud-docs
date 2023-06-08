using FluentValidation;
using USVStudDocs.Models.Secretary;
using USVStudDocs.Models.Student;

namespace USVStudDocs.BLL.Validators;

public class SecretaryCertificateUpdateItemValidator : AbstractValidator<SecretaryCertificateUpdateItem>
{
    public SecretaryCertificateUpdateItemValidator()
    {
        RuleFor(f => f.DenyReason)
            .NotEmpty()
            .WithMessage("DenyReason should not be empty");
    }
}