using System.Collections.Generic;

namespace api.Models.Common
{
    public class DataAllDto<T>
    {
        public int PageIndex { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalPages { get; set; }

        public int CurrentItemCount { get; set; }
        
        public int TotalItemCount { get; set; }

        public IList<T> Items { get; set; }

        public DataAllDto((int page, int pageSize, int totalItemCount, IList<T> items) data)
        {
            PageIndex = data.page;
            ItemsPerPage = data.pageSize;
            TotalItemCount = data.totalItemCount;
            TotalPages = data.totalItemCount / data.pageSize;
            CurrentItemCount = data.items.Count;
            Items = data.items;
            if (ItemsPerPage > 1)
            {
                TotalPages += 1;
            }
        }
    }
}