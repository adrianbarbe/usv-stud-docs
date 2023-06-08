using FluentValidation;
using USVStudDocs.DAL;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Validators
{
    public class StudentValidator : AbstractValidator<Student>
    {
        private readonly MainContext _context;

        public StudentValidator(MainContext context)
        {
            _context = context;

            RuleFor(s => s.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty");

            RuleFor(s => s.Surname)
                .NotEmpty()
                .WithMessage("Surname cannot be empty");

            RuleFor(s => s.Patronymic)
                .NotEmpty()
                .WithMessage("Patronymic cannot be empty");
            
            RuleFor(s => s.Email)
                .NotEmpty()
                .WithMessage("Email cannot be empty")
                .EmailAddress()
                .WithMessage("Email must be valid")
                .Must(UniqueEmail)
                .WithMessage("Email is already used");

            RuleFor(a => a.Faculty)
                .NotNull()
                .WithMessage("Faculty shoud not be null");

            RuleFor(a => a.YearSemester)
                .NotNull()
                .WithMessage("Semester shoud not be null");

            RuleFor(a => a.ProgramStudy)
                .NotNull()
                .WithMessage("Speciality shoud not be null");
            
        }
        
        private bool UniqueEmail(Student model, string email)
        {
            return !_context.User.Any(u => email.Trim().Equals(u.Email) && u.Id != model.Id);
        }
    }
}