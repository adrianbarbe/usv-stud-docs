using System.Collections.Generic;

namespace ePlato.CoreApp.Models.Shared.DataGrid
{
    public class RequestQueryModel
    {
        public string SortDirection { get; set; }
        public string SortBy { get; set; }
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
        public List<RequestFilterModel> Filter { get; set; }
        
        public string SearchString { get; set; }
    }
}