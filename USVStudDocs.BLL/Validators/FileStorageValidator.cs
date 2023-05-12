using FluentValidation;
using USVStudDocs.Models;

namespace USVStudDocs.BLL.Validators;

public class FileValidator : AbstractValidator<FileStorage>
{
    public FileValidator()
    {
        RuleFor(f => f.Id)
            .NotEmpty()
            .WithMessage("File id cannot be empty");
    }
}