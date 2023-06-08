using USVStudDocs.Models.Constants;

namespace USVStudDocs.Models.Admin
{
    public class YearSemester
    {
        public int Id { get; set; }
        public int YearNumber { get; set; }
        
        public FieldOfStudy FieldOfStudy { get; set; }
        
    }
}