namespace WebApi.Utilities.Pagination;

/// <summary>
/// Provides helper methods for creating paginated responses.
/// </summary>
internal sealed class PaginationHelper : IPaginationHelper
{
    private readonly IPaginationFilter _paginationFilter;
    private readonly IUriService _uriService;

    public PaginationHelper(IPaginationFilter paginationFilter, IUriService uriService)
    {
        _paginationFilter = paginationFilter;
        _uriService = uriService;
    }

    /// <summary>
    /// Creates a paginated response from the provided data.
    /// </summary>
    /// <typeparam name="T">The type of elements in the data set.</typeparam>
    /// <param name="data">The data set to paginate.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The size of each page.</param>
    /// <param name="route">The base route used for generating URIs.</param>
    /// <returns>A paginated response containing the data and related pagination details.</returns>
    public PagedResponse<IEnumerable<T>> CreatePagedResponse<T>(
        IEnumerable<T> data,
        int pageNumber,
        int pageSize,
        string route)
    {
        _paginationFilter.PageNumber = pageNumber;
        _paginationFilter.PageSize = pageSize;

        var filteredData = data
            .Skip((_paginationFilter.PageNumber - 1) * _paginationFilter.PageSize)
            .Take(_paginationFilter.PageSize)
            .ToList();

        var pagedData = new PagedData<T>
        {
            Data = filteredData,
            TotalRecords = filteredData.Count
        };

        var response = CreatePagedResponse(filteredData);
        AddPageData(response, pagedData.TotalRecords, route);

        return response;
    }

    private PagedResponse<IEnumerable<T>> CreatePagedResponse<T>(IEnumerable<T> pagedData) =>
        new(pagedData, _paginationFilter.PageNumber, _paginationFilter.PageSize);

    private void AddPageData<T>(
        PagedResponse<IEnumerable<T>> response,
        int totalRecords,
        string route)
    {
        var totalPages = CalculateTotalPages(totalRecords);

        response.NextPage = CreateNextPageUri(totalPages, _uriService, route);
        response.PreviousPage = CreatePreviousPageUri(totalPages, _uriService, route);
        response.FirstPage = _uriService.GetPageUri(route);
        response.LastPage = _uriService.GetPageUri(route);
        response.TotalPages = totalPages;
        response.TotalRecords = totalRecords;
    }

    private int CalculateTotalPages(int totalRecords)
    {
        var totalPages = totalRecords / (double)_paginationFilter.PageSize;
        return Convert.ToInt32(Math.Ceiling(totalPages));
    }

    private Uri? CreateNextPageUri(int totalPages, IUriService uriService, string route) =>
        _paginationFilter.PageNumber >= 1 && _paginationFilter.PageNumber < totalPages
            ? uriService.GetPageUri(route)
            : null;

    private Uri? CreatePreviousPageUri(int totalPages, IUriService uriService, string route) =>
        _paginationFilter.PageNumber - 1 >= 1 && _paginationFilter.PageNumber <= totalPages
            ? uriService.GetPageUri(route)
            : null;
}