using ePlato.CoreApp.Models.Shared.DataGrid;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.BLL.Extensions;
using USVStudDocs.BLL.Mappers;
using USVStudDocs.BLL.Validators;
using USVStudDocs.DAL;
using USVStudDocs.Entities;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Shared.DataGrid;

namespace USVStudDocs.BLL.Services.ProgramStudyService
{
    public class ProgramStudyService : IProgramStudyService
    {
        private readonly MainContext _context;
        private readonly IMapper<ProgramStudyEntity, ProgramStudy> _mapper;

        public ProgramStudyService(MainContext context, 
            IMapper<ProgramStudyEntity, ProgramStudy> mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public DataGridModel<ProgramStudy> GetAll(RequestQueryModel requestQueryModel)
        {
            var count = _context.ProgramStudy.Count();
            var items = _context.ProgramStudy
                .Include(f => f.Faculty);

            var itemsOrders = items
                .OrderBy(f => f.OrderBy)
                .PaginateSorting(requestQueryModel)
                .Select(f => _mapper.Map(f))
                .ToList();

            return new DataGridModel<ProgramStudy>
            {
                Items = itemsOrders,
                Total = count
            };
        }

        public List<ProgramStudy> GetAll()
        {
            return _context.ProgramStudy.OrderBy(s => s.OrderBy).Select(s => _mapper.Map(s)).ToList();
        }

        public List<ProgramStudy> GetAllByFaculty(int facultyId)
        {
            return _context.ProgramStudy
                .Include(s => s.Faculty)
                .Where(s => s.FacultyId == facultyId)
                .OrderBy(s => s.OrderBy)
                .Select(s => new ProgramStudy
                {
                    Id = s.Id,
                    Name = s.Name,
                })
                .ToList();
        }

        public ProgramStudy Get(int id)
        {
            var specialityEntity = _context.ProgramStudy
                .Include(s => s.Faculty)
                .Include(s => s.YearProgramStudy)
                    .ThenInclude(s => s.YearSemester)
                .FirstOrDefault(s => s.Id == id);
            if (specialityEntity == null)
            {
                throw new NotFoundException("Speciality not found");
            }

            return _mapper.Map(specialityEntity);
        }

        public ProgramStudy Update(ProgramStudy model)
        {
            if (model == null)
            {
                throw new ValidationException("ProgramStudy model cannot be empty");
            }

            var validator = new SpecialityValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.CreateErrorsList();

                throw new ValidationFormException(validationErrors);
            }

            if (model.Id != 0)
            {
                var specialityEntity = _context.ProgramStudy
                    .Include(f => f.Faculty)
                    .Include(f => f.YearProgramStudy)
                    .FirstOrDefault(f => f.Id == model.Id);

                if (specialityEntity == null)
                {
                    throw new NotFoundException("Speciality not found");
                }
                
                var incomingSemsIds = model.YearSemesters.Select(s => s.Id).ToArray();
                var existingSemsIds = specialityEntity.YearProgramStudy.Select(f => f.YearSemesterId).ToArray();
                var semsToDeleteIds = existingSemsIds.Except(incomingSemsIds).ToArray();
                var semsToAdd = incomingSemsIds.Except(existingSemsIds).ToArray();

                var semsEntitiesToDelete = specialityEntity.YearProgramStudy
                    .Where(f => f.ProgramStudyId == model.Id && semsToDeleteIds.Contains(f.YearSemesterId)).ToList();

                foreach (var semsEntity in semsEntitiesToDelete)
                {
                    specialityEntity.YearProgramStudy.Remove(semsEntity);
                }

                foreach (var semId in semsToAdd)
                {
                    specialityEntity.YearProgramStudy.Add(new YearProgramStudyEntity
                    {
                        YearSemesterId = semId,
                        ProgramStudyId = model.Id
                    });
                }

                specialityEntity.Name = model.Name.Trim();
                specialityEntity.NameShort = model.NameShort.Trim();
                specialityEntity.OrderBy = model.OrderBy ?? 0;
                specialityEntity.FacultyId = model.Faculty.Id;

                _context.ProgramStudy.Update(specialityEntity);
            }
            else
            {
                var specialityEntity = _mapper.Map(model);
                
                specialityEntity.YearProgramStudy = model.YearSemesters.Select(s => new YearProgramStudyEntity
                {
                    YearSemesterId = s.Id,
                    ProgramStudyId = model.Id,
                }).ToList();

                _context.ProgramStudy.Add(specialityEntity);
            }

            _context.SaveChanges();

            return model;
        }

        public void Delete(int id)
        {
            var specialityEntity = _context.ProgramStudy.FirstOrDefault(f => f.Id == id);
            if (specialityEntity == null)
            {
                throw new NotFoundException("ProgramStudy not found");
            }

            _context.ProgramStudy.Remove(specialityEntity);
            _context.SaveChanges();
        }
    }
}