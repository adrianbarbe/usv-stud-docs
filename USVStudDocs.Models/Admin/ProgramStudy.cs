namespace USVStudDocs.Models.Admin
{
    public class ProgramStudy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameShort { get; set; }
        public int? OrderBy { get; set; }
        
        public List<YearSemester>? YearSemesters { get; set; }
        public Faculty Faculty { get; set; }
        
        public FacultyPerson Secretary { get; set; }
    }
}