namespace USVStudDocs.Models.Admin
{
    public class StudentImportConcatenated
    {
        public string SurnameNamePatronymic { get; set; }
        public string Email { get; set; }
        
        public string FieldOfStudy { get; set; }
        
        public string ProgramStudy { get; set; }
        public int Year { get; set; }
        
        public string FinancialStatus { get; set; }
    }
}