using ePlato.CoreApp.Models.Shared.DataGrid;
using Microsoft.EntityFrameworkCore;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.BLL.Extensions;
using USVStudDocs.BLL.Mappers;
using USVStudDocs.BLL.Validators;
using USVStudDocs.DAL;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Authentication;
using USVStudDocs.Entities.Constants;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.BLL.Services.FacultyPersonService;

public class FacultyPersonService : IFacultyPersonService
{
    private readonly MainContext _context;
    private readonly IMapper<FacultyPersonEntity, FacultyPerson> _mapper;

    public FacultyPersonService(MainContext context, 
        IMapper<FacultyPersonEntity, FacultyPerson> mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public DataGridModel<FacultyPerson> GetAll(RequestQueryModel requestQueryModel)
    {
        var count = _context.FacultyPerson.Count();
        var items = _context.FacultyPerson.Include(f => f.User);

        var itemsOrders = items
            .PaginateSorting(requestQueryModel)
            .Select(f => _mapper.Map(f))
            .ToList();

        return new DataGridModel<FacultyPerson>
        {
            Items = itemsOrders,
            Total = count
        };
    }

    public List<FacultyPerson> GetAll()
    {
        return _context.FacultyPerson
            .Include(f => f.User)
            .OrderBy(s => s.Surname)
            .Select(s => _mapper.Map(s))
            .ToList();
    }

    public List<FacultyPerson> GetSecretaries()
    {
        return _context.FacultyPerson
            .Include(f => f.User)
            .OrderBy(s => s.Surname)
            .Where(s => s.PersonType == FacultyPersonType.Secretary)
            .Select(s => _mapper.Map(s))
            .ToList();
    }

    public FacultyPerson Get(int id)
    {
        var facultyPerson = _context.FacultyPerson
            .Include(f => f.User)
            .FirstOrDefault(f => f.Id == id);

        if (facultyPerson == null)
        {
            throw new NotFoundException("FacultyPerson not found");
        }

        return _mapper.Map(facultyPerson);
    }

    public FacultyPerson Update(FacultyPerson model)
    {
        if (model == null)
        {
            throw new ValidationException("FacultyPerson model cannot be empty");
        }

        var validator = new FacultyPersonValidator();
        var validationResult = validator.Validate(model);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors.CreateErrorsList();

            throw new ValidationFormException(validationErrors);
        }
        
        var roleEntity = _context.Role.FirstOrDefault(r => r.Name == Roles.Secretary);

        if (roleEntity == null)
        {
            throw new ValidationException("Not found Role for secretary");
        }

        if (model.Id != 0)
        {
            var facultyEntity = _context.FacultyPerson
                .Include(f => f.User)
                .FirstOrDefault(f => f.Id == model.Id);

            if (facultyEntity == null)
            {
                throw new NotFoundException("FacultyPerson not found");
            }

            facultyEntity.Prefix = model.Prefix.Trim();
            facultyEntity.Name = model.Name.Trim();
            facultyEntity.Surname = model.Surname.Trim();
            facultyEntity.Patronymic = model.Patronymic.Trim();

            var remapped = _mapper.Map(model);

            facultyEntity.PersonType = remapped.PersonType;
            
            if (model.User?.Email != "")
            {
                if (facultyEntity.User == null)
                {
                    facultyEntity.User = new UserEntity
                    {
                        Username = model.User.Email.Trim(),
                        Email = model.User.Email.Trim(),
                        RoleId = roleEntity.Id,
                    };
                }
                else
                {
                    facultyEntity.User.Username = model.User.Email.Trim();
                    facultyEntity.User.Email = model.User.Email.Trim();
                }
            }

            _context.FacultyPerson.Update(facultyEntity);
        }
        else
        {
            var facultyPersonEntity = _mapper.Map(model);

            if (model.User?.Email != "")
            {
                facultyPersonEntity.User = new UserEntity
                {
                    Username = model.User.Email.Trim(),
                    Email = model.User.Email.Trim(),
                    RoleId = roleEntity.Id,
                };
            }

            _context.FacultyPerson.Add(facultyPersonEntity);
        }

        _context.SaveChanges();

        return model;
    }

    public void Delete(int id)
    {
        var facultyPerson = _context.FacultyPerson
            .FirstOrDefault(f => f.Id == id);

        if (facultyPerson == null)
        {
            throw new NotFoundException("FacultyPerson not found");
        }

        _context.FacultyPerson.Remove(facultyPerson);
        _context.SaveChanges();
    }
}