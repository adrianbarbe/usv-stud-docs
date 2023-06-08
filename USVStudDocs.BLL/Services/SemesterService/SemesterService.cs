using USVStudDocs.BLL.Extensions;
using USVStudDocs.BLL.Mappers;
using USVStudDocs.DAL;
using USVStudDocs.Entities;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Services.SemesterService
{
    public class SemesterService : ISemesterService
    {
        private readonly MainContext _context;
        private readonly IMapper<YearSemesterEntity, Semester> _semesterMapper;

        public SemesterService(MainContext context, 
            IMapper<YearSemesterEntity, Semester> semesterMapper)
        {
            _context = context;
            _semesterMapper = semesterMapper;
        }
        
        public List<Semester> GetSemesters()
        {
            return _context.YearSemester
                .OrderBy(s => s.FieldOfStudy)
                .ThenBy(s => s.YearNumber)
                .Select(s => _semesterMapper.Map(s))
                .ToList();
        }
    }
}