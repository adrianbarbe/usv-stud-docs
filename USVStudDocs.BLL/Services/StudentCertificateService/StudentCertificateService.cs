using ePlato.CoreApp.Models.Shared.DataGrid;
using Microsoft.EntityFrameworkCore;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.BLL.Extensions;
using USVStudDocs.BLL.Mappers;
using USVStudDocs.BLL.Services.AuthorizationService;
using USVStudDocs.BLL.Validators;
using USVStudDocs.DAL;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Constants;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Shared.DataGrid;
using USVStudDocs.Models.Student;
using FieldOfStudy = USVStudDocs.Models.Constants.FieldOfStudy;
using FinancialStatus = USVStudDocs.Models.Constants.FinancialStatus;

namespace USVStudDocs.BLL.Services.StudentCertificateService;

public class StudentCertificateService : IStudentCertificateService
{
    private readonly MainContext _context;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMapper<CertificateEntity, StudentCertificateListItem> _studentCertificateListItemMapper;

    public StudentCertificateService(MainContext context, IAuthorizationService authorizationService, IMapper<CertificateEntity, StudentCertificateListItem> studentCertificateListItemMapper)
    {
        _context = context;
        _authorizationService = authorizationService;
        _studentCertificateListItemMapper = studentCertificateListItemMapper;
    }

    public DataGridModel<StudentCertificateListItem> GetList(RequestQueryModel requestQueryModel)
    {
        var userName = _authorizationService.GetUserName();

        var studentEntity = _context.Student
            .FirstOrDefault(s => s.Email == userName);
        
        if (studentEntity == null)
        {
            throw new ValidationException("Student entity is not found on this email: " + userName);
        }

        var count = _context.Certificate
            .Where(c => c.StudentId == studentEntity.Id)
            .OrderBy(c => c.RegistrationDate)
            .Count();
        
        var items = _context.Certificate
            .Where(c => c.StudentId == studentEntity.Id)
            .OrderBy(c => c.RegistrationDate);

        var itemsOrders = items
            .Include(c => c.Secretary)
            .PaginateSorting(requestQueryModel)
            .Select(f => _studentCertificateListItemMapper.Map(f))
            .ToList();

        return new DataGridModel<StudentCertificateListItem>
        {
            Items = itemsOrders,
            Total = count
        };
    }

    public void CreateCertificate(StudentCertificateCreateItem model)
    {
        if (model == null)
        {
            throw new ValidationException("Faculty model cannot be empty");
        }

        var validator = new StudentCertificateCreateItemValidator();
        var validationResult = validator.Validate(model);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors.CreateErrorsList();

            throw new ValidationFormException(validationErrors);
        }
        
        var userName = _authorizationService.GetUserName();

        var studentEntity = _context.Student
            .Include(s => s.ProgramOfStudy)
            .FirstOrDefault(s => s.Email == userName);
        
        if (studentEntity == null)
        {
            throw new ValidationException("Student entity is not found on this email: " + userName);
        }

        var currentStudentCertificatesCount = _context.Certificate.Count(c => c.StudentId == studentEntity.Id && c.Status == CertificateStatus.New);

        if (currentStudentCertificatesCount > 0)
        {
            throw new ValidationException("There are already pending certificate requests. You cannot create another one until the previous is fulfilled.");
        }

        if (studentEntity.ProgramOfStudy == null)
        {
            throw new ValidationException("No program of study assigned. Please contact dean office.");
        }

        var certificateEntity = new CertificateEntity
        {
            StudentId = studentEntity.Id,
            Status = CertificateStatus.New,
            RegistrationDate = DateTime.Now,
            CertificateReason = model.Reason,
            SecretaryId = studentEntity.ProgramOfStudy.SecretaryId,
        };

        _context.Certificate.Add(certificateEntity);
        _context.SaveChanges();
    }

    public Student GetCurrentStudentInfo()
    {
        var userName = _authorizationService.GetUserName();

        var studentEntity = _context.Student
            .Include(s => s.Faculty)
            .Include(s => s.YearSemester)
            .Include(s => s.ProgramOfStudy)
            .FirstOrDefault(s => s.Email == userName);

        FinancialStatus financialStatus = FinancialStatus.Budget;

        switch (studentEntity.FinancialStatus)
        {
            case Entities.Constants.FinancialStatus.Budget:
                financialStatus = FinancialStatus.Budget;
                break;
            case Entities.Constants.FinancialStatus.TuitionFee:
                financialStatus = FinancialStatus.TuitionFee;
                break;
        }

        FieldOfStudy fieldOfStudy = FieldOfStudy.Bachelor;

        switch (studentEntity.FieldOfStudy)
        {
            case Entities.Constants.FieldOfStudy.Bachelor:
                fieldOfStudy = FieldOfStudy.Bachelor;
                break;
            case Entities.Constants.FieldOfStudy.Master:
                fieldOfStudy = FieldOfStudy.Master;
                break;
            case Entities.Constants.FieldOfStudy.ProfessionalConversion:
                fieldOfStudy = FieldOfStudy.ProfessionalConversion;
                break;
        }

        return new Student
        {
            Id = studentEntity.Id,
            Surname = studentEntity.Surname,
            Patronymic = studentEntity.Patronymic,
            Name = studentEntity.Name,
            Email = studentEntity.Email,
            FinancialStatus = financialStatus,
            Faculty = new Faculty
            {
                Id = studentEntity.Faculty.Id,
                Name = studentEntity.Faculty.Name,
                NameShort = studentEntity.Faculty.NameShort,
            },
            ProgramStudy = new ProgramStudy
            {
                Id = studentEntity.ProgramOfStudy.Id,
                Name = studentEntity.ProgramOfStudy.Name,
                NameShort = studentEntity.ProgramOfStudy.NameShort,
            },
            YearSemester = new Semester
            {
                Id = studentEntity.YearSemester.Id,
                YearNumber = studentEntity.YearSemester.YearNumber,
                FieldOfStudy = fieldOfStudy,
            }
        };
    }
}