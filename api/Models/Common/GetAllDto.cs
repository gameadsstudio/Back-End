using System.Collections.Generic;

namespace api.Models.Common
{
    public class GetAllDto<T>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int MaxPage { get; set; }

        public int Count { get; set; }

        public List<T> Items { get; set; }
    }
}