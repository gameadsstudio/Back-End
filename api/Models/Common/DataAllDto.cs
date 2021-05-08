using System.Collections.Generic;

namespace api.Models.Common
{
    public class DataAllDto<T>
    {
        public int PageIndex { get; set; }
        
        public int ItemsPerPage { get; set; }
        
        public int TotalPages { get; set; }
        
        public int CurrentItemCount { get; set; }
        
        public List<T> Items { get; set; }
    }
}