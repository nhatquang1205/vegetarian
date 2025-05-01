namespace vegetarian.Commons.Schemas
{
    public class PaginationRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "Id";
        public string OrderDirection { get; set; } = "asc";
        public int Skip => (Page - 1) * PageSize;
    }
}