using Microsoft.EntityFrameworkCore;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.BLL.Extensions;
using USVStudDocs.BLL.Services.AuthorizationService;
using USVStudDocs.BLL.Validators;
using USVStudDocs.DAL;
using USVStudDocs.Entities;
using USVStudDocs.Models.Secretary;

namespace USVStudDocs.BLL.Services.CommonNumberService;

public class CommonNumberService : ICommonNumberService
{
    private readonly MainContext _context;
    private readonly IAuthorizationService _authorizationService;

    public CommonNumberService(MainContext context, IAuthorizationService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    public CommonNumber GetLatest()
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

        var latestNumber = _context.CommonRegistrationNumber
            .Where(c => c.FacultyId == programStudy.FacultyId)
            .OrderByDescending(c => c.Date).FirstOrDefault();

        return new CommonNumber
        {
            Number = latestNumber?.RegistrationNumber ?? null,
        };
    }
    
    public CommonNumber GetToday()
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

        var latestNumber = _context.CommonRegistrationNumber
            .Where(c => c.FacultyId == programStudy.FacultyId && c.Date.Date == DateTime.Now.Date)
            .OrderByDescending(c => c.Date).FirstOrDefault();

        return new CommonNumber
        {
            Number = latestNumber?.RegistrationNumber ?? null,
            OrderNumber = latestNumber?.OrderNumber,
        };
    }

    public void Save(CommonNumber model)
    {
        if (model == null)
        {
            throw new ValidationException("Faculty model cannot be empty");
        }

        var validator = new CommonNumberValidator(_context);
        var validationResult = validator.Validate(model);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors.CreateErrorsList();

            throw new ValidationFormException(validationErrors);
        }
        
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

        var commonRegistrationNumberEntity = new CommonRegistrationNumberEntity
        {
            OrderNumber = 1,
            RegistrationNumber = model.Number,
            Date = DateTime.Now,
            FacultyId = programStudy.FacultyId,
        };

        _context.CommonRegistrationNumber.Add(commonRegistrationNumberEntity);
        _context.SaveChanges();
    }
}