using System.Collections.Generic;

namespace api.Models.Common
{
    public class GetAllDto<T>
    {
        public DataAllDto<T> Data { get; set; }

        public GetAllDto((int page, int pageSize, int maxPage, IList<T> items) data)
        {
            Data = new DataAllDto<T>(data);
        }
    }
}