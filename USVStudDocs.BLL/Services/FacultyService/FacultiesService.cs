using ePlato.CoreApp.Models.Shared.DataGrid;
using Microsoft.EntityFrameworkCore;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.BLL.Extensions;
using USVStudDocs.BLL.Mappers;
using USVStudDocs.BLL.Validators;
using USVStudDocs.DAL;
using USVStudDocs.Entities;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.BLL.Services.FacultyService
{
    public class FacultiesService : IFacultiesService
    {
        private readonly MainContext _context;
        private readonly IMapper<FacultyEntity, Faculty> _mapper;

        public FacultiesService(MainContext context, 
            IMapper<FacultyEntity, Faculty> mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public DataGridModel<Faculty> GetAll(RequestQueryModel requestQueryModel)
        {
            var count = _context.Faculty.Count();
            var items = _context.Faculty;

            var itemsOrders = items
                .Include(f => f.Dean)
                .Include(f => f.SecretaryHead)
                .PaginateSorting(requestQueryModel)
                .Select(f => _mapper.Map(f))
                .ToList();

            return new DataGridModel<Faculty>
            {
                Items = itemsOrders,
                Total = count
            };
        }

        public List<Faculty> GetAll()
        {
            return _context.Faculty
                .Include(f => f.Dean)
                .Include(f => f.SecretaryHead)
                .OrderBy(s => s.OrderBy)
                .Select(s => _mapper.Map(s))
                .ToList();
        }

        
        public Faculty Get(int id)
        {
            var faculty = _context.Faculty
                .Include(f => f.Dean)
                .Include(f => f.SecretaryHead)
                .FirstOrDefault(f => f.Id == id);

            if (faculty == null)
            {
                throw new NotFoundException("Faculty not found");
            }

            return _mapper.Map(faculty);
        }

        public Faculty Update(Faculty model)
        {
            if (model == null)
            {
                throw new ValidationException("Faculty model cannot be empty");
            }

            var validator = new FacultyValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.CreateErrorsList();

                throw new ValidationFormException(validationErrors);
            }

            if (model.Id != 0)
            {
                var facultyEntity = _context.Faculty
                    .FirstOrDefault(f => f.Id == model.Id);

                if (facultyEntity == null)
                {
                    throw new NotFoundException("Faculty not found");
                }

                facultyEntity.Name = model.Name.Trim();
                facultyEntity.NameShort = model.NameShort.Trim();
                facultyEntity.OrderBy = model.OrderBy ?? 0;

                facultyEntity.DeanId = model.Dean.Id;
                facultyEntity.SecretaryHeadId = model.SecretaryHead.Id;

                _context.Faculty.Update(facultyEntity);
            }
            else
            {
                var facultyEntity = _mapper.Map(model);

                _context.Faculty.Add(facultyEntity);
            }

            _context.SaveChanges();

            return model;
        }

        public void Delete(int id)
        {
            var faculty = _context.Faculty
                .FirstOrDefault(f => f.Id == id);

            if (faculty == null)
            {
                throw new NotFoundException("Faculty not found");
            }

            _context.Faculty.Remove(faculty);
            _context.SaveChanges();
        }
    }
}