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

namespace USVStudDocs.BLL.Services.StudentService;

public class StudentService : IStudentService
{
    private readonly MainContext _context;
    private readonly IMapper<StudentEntity, Student> _mapper;

    public StudentService(MainContext context, IMapper<StudentEntity, Student> mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public DataGridModel<Student> GetAllGrid(int facultyId, int specialityId, int semesterId)
    {
        if (facultyId == 0 || specialityId == 0 || semesterId == 0)
        {
            throw new ValidationException("Cannot get students list. Faculty, speciality, semester not speciafied.");
        }

        var count = _context.Student
            .Count(a => a.FacultyId == facultyId &&
                        a.ProgramOfStudyId == specialityId &&
                        a.YearSemesterId == semesterId);

        var items = _context.Student;

        var itemsOrders = items
            .Where(a => a.FacultyId == facultyId
                        && a.ProgramOfStudyId == specialityId
                        && a.YearSemesterId == semesterId)
            .OrderBy(g => g.Surname)
            .Select(s => _mapper.Map(s))
            .ToList();

        return new DataGridModel<Student>
        {
            Items = itemsOrders,
            Total = count
        };
    }

    public List<Student> GetAll()
    {
        return _context.Student
            .OrderBy(a => a.Surname)
            .Select(a => _mapper.Map(a))
            .ToList();
    }

    public List<Student> GetAll(int facultyId, int specialityId, int semesterId)
    {
        return _context.Student
            .Where(a => a.FacultyId == facultyId
                        && a.ProgramOfStudyId == specialityId
                        && a.YearSemesterId == semesterId)
            .OrderBy(a => a.Surname)
            .Select(a => _mapper.Map(a))
            .ToList();
    }

    public Student Get(int id)
    {
        return _context.Student
            .Where(a => a.Id == id)
            .Select(a => _mapper.Map(a))
            .FirstOrDefault();
    }

    public Student Update(Student model)
    {
        if (model == null)
        {
            throw new ValidationException("Student model cannot be empty");
        }

        var roleEntity = _context.Role.FirstOrDefault(r => r.Name == Roles.Student);

        if (roleEntity == null)
        {
            throw new ValidationException("Not found Role for student");
        }
        
        var validator = new StudentValidator(_context);
        var validationResult = validator.Validate(model);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors.CreateErrorsList();

            throw new ValidationFormException(validationErrors);
        }

        if (model.Id != 0)
        {
            var studentEntity = _context.Student
                .FirstOrDefault(f => f.Id == model.Id);

            if (studentEntity == null)
            {
                throw new NotFoundException("Student not found");
            }

            studentEntity.Surname = model.Surname.Trim();
            studentEntity.Name = model.Name.Trim();
            studentEntity.Patronymic = model.Patronymic.Trim();
            studentEntity.Email = model.Email.Trim();
            studentEntity.FacultyId = model.Faculty.Id;
            studentEntity.YearSemesterId = model.YearSemester.Id;
            studentEntity.ProgramOfStudyId = model.ProgramStudy.Id;
            
            if (model?.Email != "")
            {
                if (studentEntity.User == null)
                {
                    studentEntity.User = new UserEntity
                    {
                        Username = model.Email.Trim(),
                        Email = model.Email.Trim(),
                        RoleId = roleEntity.Id,
                    };
                }
                else
                {
                    studentEntity.User.Username = model.Email.Trim();
                    studentEntity.User.Email = model.Email.Trim();
                }
            }

            _context.Student.Update(studentEntity);
        }
        else
        {
            var studentEntity = _mapper.Map(model);

            studentEntity.User = new UserEntity
            {
                Email = studentEntity.Email.Trim(),
                Username = studentEntity.Email.Trim(),
                RoleId = roleEntity.Id,
            };

            _context.Student.Add(studentEntity);
        }

        _context.SaveChanges();

        return model;
    }

    public void Delete(int id)
    {
        var studentEntity = _context.Student
            .Include(s => s.User)
            .FirstOrDefault(f => f.Id == id);
        
        if (studentEntity == null)
        {
            throw new NotFoundException("Student not found");
        }

        _context.Student.Remove(studentEntity);
        _context.User.Remove(studentEntity.User);
        
        _context.SaveChanges();
    }

    public void DeleteMany(List<Student> students)
    {
        var incomingStudents = students.Select(a => a.Id).ToArray();

        if (incomingStudents.Length == 0)
        {
            throw new NotFoundException($"Not found students with ids {incomingStudents}");
        }

        var studentEntities = _context.Student
            .Include(s => s.User)
            .Where(a => incomingStudents.Contains(a.Id))
            .ToList();

        foreach (var studentEntity in studentEntities)
        {
            _context.Student.Remove(studentEntity);
            _context.User.Remove(studentEntity.User);
        }
        
        _context.SaveChanges();
    }
}