using System.Collections.Generic;

namespace api.Models.Common
{
    public class GetAllDto<T>
    {
        public int Status { get; set; } = 200;

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int MaxPage { get; set; }

        public List<T> Result { get; set; }
    }
}