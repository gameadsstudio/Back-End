namespace api.Helpers
{
    public class PagingDto
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 50;
    }

    public static class PagingHelper
    {
        public static PagingDto Check(PagingDto paging)
        {
            if (paging.page < 1) paging.page = 0;
            if (paging.pageSize < 1 || paging.pageSize > 50) paging.pageSize = 50;
            return paging;
        }
    }
}