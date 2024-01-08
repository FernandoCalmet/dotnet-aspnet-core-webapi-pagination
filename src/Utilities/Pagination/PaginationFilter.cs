using Microsoft.Extensions.Options;

namespace WebApi.Utilities.Pagination;

/// <summary>
/// Represents the parameters for pagination, including the page number and page size.
/// This class provides a standardized way to manage pagination requests.
/// </summary>
internal sealed class PaginationFilter : IPaginationFilter
{
    private readonly PaginationOptions _paginationOptions;

    private int _pageNumber;
    private int _pageSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationFilter"/> class with default pagination settings.
    /// </summary>
    public PaginationFilter(IOptions<PaginationOptions> paginationOptions)
    {
        _paginationOptions = paginationOptions.Value;
        SetDefaultValues();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationFilter"/> class with the specified page number and page size.
    /// </summary>
    /// <param name="paginationOptions"></param>
    /// <param name="pageNumber">The page number for the pagination.</param>
    /// <param name="pageSize">The size of each page for the pagination.</param>
    public PaginationFilter(IOptions<PaginationOptions> paginationOptions, int pageNumber, int pageSize)
    {
        _paginationOptions = paginationOptions.Value;
        PageNumber = pageNumber == 0 ? _paginationOptions.DefaultPageNumber : pageNumber;
        PageSize = pageSize == 0 ? _paginationOptions.DefaultPageSize : pageSize;
    }

    /// <summary>
    /// Gets or sets the page number. The value is always greater than or equal to 1.
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value <= 0 ? _paginationOptions.DefaultPageNumber : value;
    }

    /// <summary>
    /// Gets or sets the page size. The value is always greater than or equal to 1 and less than or equal to the maximum page size.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value <= 0 || value > _paginationOptions.MaxPageSize) ? _paginationOptions.DefaultPageSize : value;
    }

    private void SetDefaultValues()
    {
        _pageNumber = _paginationOptions.DefaultPageNumber;
        _pageSize = _paginationOptions.DefaultPageSize;
    }
}