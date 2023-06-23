using Microsoft.EntityFrameworkCore;
using USVStudDocs.DAL;
using USVStudDocs.Entities;
using USVStudDocs.Entities.Constants;
using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Services.NavigationService;

public class NavigationService : INavigationService
{
    private readonly MainContext _context;

    public NavigationService(MainContext context)
    {
        _context = context;
    }
    
    public List<Faculty> GetNavigation()
        {
            var faculties = _context.Faculty.OrderBy(f => f.OrderBy)
                .Include(f => f.ProgramStudies)
                .ToList();
            
            var specialities = _context.ProgramStudy
                .Include(s => s.YearProgramStudy)
                .OrderBy(s => s.OrderBy).ToList();
            
            var semesters = _context.YearSemester
                .ToList();
            
            List<Faculty> navList = faculties.Select(f =>
                new Faculty() {
                    Id = f.Id,
                    Name = f.Name,
                    NameShort = f.NameShort,
                    OrderBy = f.OrderBy,
                    ProgramStudies = specialities
                        .Where(s => s.FacultyId == f.Id)
                        .OrderBy(s => s.OrderBy)
                        .Select(s => new ProgramStudy
                        {
                            Id = s.Id,
                            Name = s.Name,
                            NameShort = s.NameShort,
                            OrderBy = s.OrderBy,
                            YearSemesters = semesters
                                .Where(sem => s.YearProgramStudy.Select(y => y.YearSemesterId).Contains(sem.Id))
                                .OrderBy(sem => sem.YearNumber)
                                .Select(sem => new YearSemester
                                {
                                    Id = sem.Id,
                                    YearNumber = sem.YearNumber,
                                    FieldOfStudy = MapFieldOfStudy(sem),
                                })
                                .ToList()
                        })
                        .ToList()
                })
            .ToList();

            return navList;
        }

    private Models.Constants.FieldOfStudy MapFieldOfStudy(YearSemesterEntity source)
    {
        Models.Constants.FieldOfStudy fieldOfStudy = Models.Constants.FieldOfStudy.Bachelor;

        switch (source.FieldOfStudy)
        {
            case FieldOfStudy.Bachelor:
                fieldOfStudy = Models.Constants.FieldOfStudy.Bachelor;
                break;
            case FieldOfStudy.Master:
                fieldOfStudy = Models.Constants.FieldOfStudy.Master;
                break;
            case FieldOfStudy.ProfessionalConversion:
                fieldOfStudy = Models.Constants.FieldOfStudy.ProfessionalConversion;
                break;
        }

        return fieldOfStudy;
    }
}