using WebApi.Contracts;

namespace WebApi.Helpers;

public class PaginationHelper : IPaginationHelper
{
    public PagedResponse<List<T>> CreatePagedResponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
    {
        var response = CreatePagedResponse(pagedData, validFilter);
        AddPageData(response, validFilter, totalRecords, uriService, route);

        return response;
    }

    private static PagedResponse<List<T>> CreatePagedResponse<T>(List<T> pagedData, PaginationFilter validFilter)
    {
        return new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
    }

    private static void AddPageData<T>(PagedResponse<List<T>> response, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
    {
        var totalPages = CalculateTotalPages(totalRecords, validFilter.PageSize);

        response.NextPage = CreateNextPageUri(validFilter, totalPages, uriService, route);
        response.PreviousPage = CreatePreviousPageUri(validFilter, totalPages, uriService, route);
        response.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
        response.LastPage = uriService.GetPageUri(new PaginationFilter(totalPages, validFilter.PageSize), route);
        response.TotalPages = totalPages;
        response.TotalRecords = totalRecords;
    }

    private static int CalculateTotalPages(int totalRecords, int pageSize)
    {
        var totalPages = (totalRecords / (double)pageSize);
        return Convert.ToInt32(Math.Ceiling(totalPages));
    }

    private static Uri CreateNextPageUri(PaginationFilter filter, int totalPages, IUriService uriService, string route)
    {
        return filter.PageNumber >= 1 && filter.PageNumber < totalPages
            ? uriService.GetPageUri(new PaginationFilter(filter.PageNumber + 1, filter.PageSize), route)
            : null;
    }

    private static Uri CreatePreviousPageUri(PaginationFilter filter, int totalPages, IUriService uriService, string route)
    {
        return filter.PageNumber - 1 >= 1 && filter.PageNumber <= totalPages
            ? uriService.GetPageUri(new PaginationFilter(filter.PageNumber - 1, filter.PageSize), route)
            : null;
    }
}