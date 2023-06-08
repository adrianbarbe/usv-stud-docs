namespace USVStudDocs.Models.Admin
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameShort { get; set; }
        public int? OrderBy { get; set; }

        public FacultyPerson Dean { get; set; }
        public FacultyPerson SecretaryHead { get; set; }

        public List<ProgramStudy> ProgramStudies { get; set; }
        
    }
}