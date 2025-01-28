

namespace Ally.Domain.Dtos
{
    public class Pagination
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

    public class PaginatedDto : Pagination
    {
        public Object data { get; set; }
    }
}
