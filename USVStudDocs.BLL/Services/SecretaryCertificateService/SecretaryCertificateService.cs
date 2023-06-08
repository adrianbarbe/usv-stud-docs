
using System.Text;
using ePlato.CoreApp.Models.Shared.DataGrid;
using Microsoft.EntityFrameworkCore;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.BLL.Extensions;
using USVStudDocs.BLL.Mappers;
using USVStudDocs.BLL.Services.AuthorizationService;
using USVStudDocs.BLL.Services.CommonNumberService;
using USVStudDocs.BLL.Validators;
using USVStudDocs.DAL;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Resources;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Constants;
using USVStudDocs.Models.Secretary;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.BLL.Services.SecretaryCertificateService;

public class SecretaryCertificateService : ISecretaryCertificateService
{
    private readonly MainContext _context;
    private readonly IMapper<CertificateEntity, SecretaryCertificateListItem> _secretaryCertificateMapper;
    private readonly IAuthorizationService _authorizationService;

    public SecretaryCertificateService(MainContext context, 
        IMapper<CertificateEntity, SecretaryCertificateListItem> secretaryCertificateMapper,
        IAuthorizationService authorizationService)
    {
        _context = context;
        _secretaryCertificateMapper = secretaryCertificateMapper;
        _authorizationService = authorizationService;
    }

    public SecretaryCertificatePrint GetPrint(int id)
    {
        var foundCertificate = _context.Certificate
            .Include(c => c.Secretary)
            
            .Include(c => c.Student)
            .ThenInclude(cs => cs.Faculty)
            
            .Include(c => c.Student)
            .ThenInclude(cs => cs.ProgramOfStudy)
            
            .Include(c => c.Student)
            .ThenInclude(cs => cs.YearSemester)
            .FirstOrDefault(c => c.Id == id);

        if (foundCertificate == null)
        {
            throw new ValidationException("Certificate not found");
        }

        var settingsKeys = new[]
        {
            SettingsKeys.SemesterSettings.EducationalYearStartDate.Key,
        };

        var settings = _context.Settings.Where(s => settingsKeys.Contains(s.Key)).ToList();

        var educationalYearStartDateStr = settings
            .FirstOrDefault(s => s.Key == SettingsKeys.SemesterSettings.EducationalYearStartDate.Key);

        if (string.IsNullOrEmpty(educationalYearStartDateStr?.Value))
        {
            throw new ValidationException("Educational year is not set in settings");
        }

        var educationalYearStartDate = educationalYearStartDateStr.ParseToDateTime();

        var faculty = _context.Faculty
            .Where(f => f.Id == foundCertificate.Student.FacultyId)
            .Include(f => f.Dean)
            .Include(f => f.SecretaryHead)
            .FirstOrDefault();

        if (faculty == null)
        {
            throw new ValidationException("Faculty not found");
        }

        var programStudy = _context.ProgramStudy
            .Where(p => p.Id == foundCertificate.Student.ProgramOfStudyId)
            .Include(p => p.Secretary)
            .FirstOrDefault();

        if (programStudy == null)
        {
            throw new ValidationException("Not found program study");
        }

        return new SecretaryCertificatePrint
        {
            CertificateItem = _secretaryCertificateMapper.Map(foundCertificate),
            StudyYearStart = educationalYearStartDate.Year,
            Dean = new FacultyPerson
            {
                Id = faculty.Dean.Id,
                Prefix = faculty.Dean.Prefix,
                Surname = faculty.Dean.Surname,
                Name = faculty.Dean.Name,
                Patronymic = faculty.Dean.Patronymic,
            },
            SecretaryHead = new FacultyPerson
            {
                Id = faculty.SecretaryHead.Id,
                Prefix = faculty.SecretaryHead.Prefix,
                Surname = faculty.SecretaryHead.Surname,
                Name = faculty.SecretaryHead.Name,
                Patronymic = faculty.SecretaryHead.Patronymic,
            },
            
            Secretary = new FacultyPerson
            {
                Id = programStudy.Secretary.Id,
                Prefix = programStudy.Secretary.Prefix,
                Surname = programStudy.Secretary.Surname,
                Name = programStudy.Secretary.Name,
                Patronymic = programStudy.Secretary.Patronymic,
            },
        };
    }

    public DataGridModel<SecretaryCertificateListItem> GetList(RequestQueryModel requestQueryModel, CertificateStatus status)
    {
        var userName = _authorizationService.GetUserName();

        var personFaculty = _context.FacultyPerson
            .Include(fp => fp.User)
            .FirstOrDefault(fp => fp.User.Email == userName);
        
        if (personFaculty == null)
        {
            throw new ValidationException("Cannot find personFaculty");
        }

        var programStudy = _context.ProgramStudy
            .FirstOrDefault(ps => ps.SecretaryId == personFaculty.Id);

        if (programStudy == null)
        {
            throw new ValidationException("Cannot find program study");
        }
        
        Entities.Constants.CertificateStatus certificateStatus = Entities.Constants.CertificateStatus.New;

        switch (status)
        {
            case CertificateStatus.New:
                certificateStatus = Entities.Constants.CertificateStatus.New;
                break;
            
            case CertificateStatus.Approved:
                certificateStatus = Entities.Constants.CertificateStatus.Approved;
                break;
            
            case CertificateStatus.Denied:
                certificateStatus = Entities.Constants.CertificateStatus.Denied;
                break;
            
            case CertificateStatus.Printed:
                certificateStatus = Entities.Constants.CertificateStatus.Printed;
                break;
            
            case CertificateStatus.Signed:
                certificateStatus = Entities.Constants.CertificateStatus.Signed;
                break;
        }

        var count = _context.Certificate
            .Include(c => c.Student)
            .Where(c => c.Student.FacultyId == programStudy.FacultyId && c.Status == certificateStatus)
            .OrderBy(c => c.RegistrationDate)
            .Count();
        
        var items = _context.Certificate
            .OrderBy(c => c.RegistrationDate);

        var itemsOrders = items
            .Include(c => c.Secretary)
            
            .Include(c => c.Student)
            .ThenInclude(cs => cs.Faculty)
            
            .Include(c => c.Student)
            .ThenInclude(cs => cs.ProgramOfStudy)
            
            .Include(c => c.Student)
            .ThenInclude(cs => cs.YearSemester)
            
            .Where(c => c.Student.FacultyId == programStudy.FacultyId && c.Status == certificateStatus)

            .PaginateSorting(requestQueryModel)
            .Select(f => _secretaryCertificateMapper.Map(f))
            .ToList();

        return new DataGridModel<SecretaryCertificateListItem>
        {
            Items = itemsOrders,
            Total = count
        };
    }

    public SecretaryCertificateSummary GetCount()
    {
        var userName = _authorizationService.GetUserName();

        var personFaculty = _context.FacultyPerson
            .Include(fp => fp.User)
            .FirstOrDefault(fp => fp.User.Email == userName);
        
        if (personFaculty == null)
        {
            throw new ValidationException("Cannot find personFaculty");
        }

        var programStudy = _context.ProgramStudy
            .FirstOrDefault(ps => ps.SecretaryId == personFaculty.Id);

        if (programStudy == null)
        {
            throw new ValidationException("Cannot find program study");
        }

        var certificates = _context.Certificate
            .Include(c => c.Student)
            .Where(c => c.Student.FacultyId == programStudy.FacultyId)
            .ToList();

        return new SecretaryCertificateSummary
        {
            New = certificates.Count(c => c.Status == Entities.Constants.CertificateStatus.New),
            Approved = certificates.Count(c => c.Status == Entities.Constants.CertificateStatus.Approved),
            Printed = certificates.Count(c => c.Status == Entities.Constants.CertificateStatus.Printed),
        };
    }

    public void ConfirmItem(int id)
    {
        var foundCertificate = _context.Certificate
            .Include(c => c.Student)
            .FirstOrDefault(c => c.Id == id);

        if (foundCertificate == null)
        {
            throw new ValidationException("Certificate not found");
        }

        foundCertificate.RegistrationNumber = GenerateRegistrationNumber(foundCertificate.Student.FacultyId);
        foundCertificate.ApprovedDate = DateTime.Now;
        foundCertificate.Status = Entities.Constants.CertificateStatus.Approved;
        _context.Certificate.Update(foundCertificate);
        _context.SaveChanges();
        
        
        // Send email
    }

    public void ConfirmPrint(int id)
    {
        var foundCertificate = _context.Certificate.FirstOrDefault(c => c.Id == id);

        if (foundCertificate == null)
        {
            throw new ValidationException("Certificate not found");
        }

        foundCertificate.Status = Entities.Constants.CertificateStatus.Printed;
        _context.Certificate.Update(foundCertificate);
        _context.SaveChanges();
    }

    public void ConfirmSignature(int id)
    {
        var foundCertificate = _context.Certificate.FirstOrDefault(c => c.Id == id);

        if (foundCertificate == null)
        {
            throw new ValidationException("Certificate not found");
        }

        foundCertificate.Status = Entities.Constants.CertificateStatus.Signed;
        _context.Certificate.Update(foundCertificate);
        _context.SaveChanges();
        
        // Send email
    }

    public void RejectItem(int id, SecretaryCertificateUpdateItem model)
    {
        var foundCertificate = _context.Certificate.FirstOrDefault(c => c.Id == id);

        if (foundCertificate == null)
        {
            throw new ValidationException("Certificate not found");
        }
        
        var validator = new SecretaryCertificateUpdateItemValidator();
        var validationResult = validator.Validate(model);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors.CreateErrorsList();

            throw new ValidationFormException(validationErrors);
        }

        foundCertificate.Status = Entities.Constants.CertificateStatus.Denied;
        foundCertificate.DenyReason = model.DenyReason;
        
        _context.Certificate.Update(foundCertificate);
        _context.SaveChanges();
    }

    private string GenerateRegistrationNumber(int facultyId)
    {
        List<string> registrationNumber = new List<string>();
        
        var latestNumber = _context.CommonRegistrationNumber
            .Where(c => c.FacultyId == facultyId && c.Date.Date == DateTime.Now.Date)
            .OrderByDescending(c => c.Date)
            .FirstOrDefault();

        if (latestNumber?.RegistrationNumber == null)
        {
            throw new ValidationException("No common registration number for today");
        }

        var facultyEntity = _context.Faculty.FirstOrDefault(f => f.Id == facultyId);

        if (facultyEntity == null)
        {
            throw new ValidationException("No faculty found");
        }

        registrationNumber.Add(latestNumber.RegistrationNumber);
        registrationNumber.Add(latestNumber.OrderNumber.ToString() ?? string.Empty);
        registrationNumber.Add(facultyEntity.NameShort);
        registrationNumber.Add(DateTime.Now.ToString("dd.MM.yyyy"));

        latestNumber.OrderNumber += 1;
        _context.CommonRegistrationNumber.Update(latestNumber);
        _context.SaveChanges();

        return string.Join("/", registrationNumber);
    }
}