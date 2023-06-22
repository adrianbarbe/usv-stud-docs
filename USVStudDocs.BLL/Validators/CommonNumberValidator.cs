using FluentValidation;
using USVStudDocs.DAL;
using USVStudDocs.Models.Secretary;

namespace USVStudDocs.BLL.Validators;

public class CommonNumberValidator : AbstractValidator<CommonNumber>
{
    private readonly MainContext _context;

    public CommonNumberValidator(MainContext context)
    {
        _context = context;

        RuleFor(f => f.Number)
            .NotEmpty()
            .WithMessage("Number should not be empty")
            .NotNull()
            .WithMessage("Number should not be empty")
            .Must(UniqueNumber)
            .WithMessage("Number must be unique");
    }
    
    private bool UniqueNumber(CommonNumber model, string number)
    {
        return !_context.CommonRegistrationNumber.Any(u => number.Trim().Equals(u.RegistrationNumber));
    }
}