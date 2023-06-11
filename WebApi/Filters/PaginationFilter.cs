namespace WebApi.Filters;

public class PaginationFilter
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 10;

    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public PaginationFilter()
    {
        PageNumber = DefaultPageNumber;
        PageSize = DefaultPageSize;
    }

    public PaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < DefaultPageNumber ? DefaultPageNumber : pageNumber;
        PageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
    }
}