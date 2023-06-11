using WebApi.Contracts;

namespace WebApi.Helpers;

public interface IPaginationHelper
{
    PagedResponse<List<T>> CreatePagedResponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route);
}