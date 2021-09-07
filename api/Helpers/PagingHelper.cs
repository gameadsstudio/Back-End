namespace api.Helpers
{
    public class PagingDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    public static class PagingHelper
    {
        public static PagingDto Check(PagingDto paging)
        {
            if (paging.Page < 1) paging.Page = 1;
            if (paging.PageSize < 1 || paging.PageSize > 50) paging.PageSize = 50;
            return paging;
        }
    }
}
