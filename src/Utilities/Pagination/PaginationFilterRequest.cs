namespace WebApi.Utilities.Pagination;

public sealed record PaginationFilterRequest(int PageNumber, int PageSize);