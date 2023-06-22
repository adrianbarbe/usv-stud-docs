namespace USVStudDocs.Models.Shared.DataGrid
{
    public class DataGridModel<T>
    {
        public List<T> Items { get; set; }
        public int Total { get; set; }
    }
}