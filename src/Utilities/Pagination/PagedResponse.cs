namespace WebApi.Utilities.Pagination;

/// <summary>
/// Represents a paginated response that extends the generic response class.
/// This class encapsulates the pagination-related details along with the response data.
/// </summary>
/// <typeparam name="T">The type of the data in the response.</typeparam>
public class PagedResponse<T> : CustomResponse<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResponse{T}"/> class with specified data, page number, and page size.
    /// </summary>
    /// <param name="data">The data for the response.</param>
    /// <param name="pageNumber">The page number of the response.</param>
    /// <param name="pageSize">The size of each page.</param>
    public PagedResponse(T data, int pageNumber, int pageSize)
        : base(data)
    {
        PageNumber = EnsureValidPageNumber(pageNumber);
        PageSize = EnsureValidPageSize(pageSize);
    }

    /// <summary>
    /// Gets the page number for this response.
    /// This represents the current page in a paginated response.
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// Gets the size of each page.
    /// This represents the number of items per page in a paginated response.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets or sets the URI of the first page in the paginated series.
    /// This can be used to navigate to the first page.
    /// </summary>
    public Uri? FirstPage { get; set; }

    /// <summary>
    /// Gets or sets the URI of the last page in the paginated series.
    /// This can be used to navigate to the last page.
    /// </summary>
    public Uri? LastPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages available in the paginated response.
    /// </summary>
    public int? TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total number of records available across all pages.
    /// </summary>
    public int? TotalRecords { get; set; }

    /// <summary>
    /// Gets or sets the URI of the next page in the paginated series.
    /// This can be used to navigate to the next page.
    /// </summary>
    public Uri? NextPage { get; set; }

    /// <summary>
    /// Gets or sets the URI of the previous page in the paginated series.
    /// This can be used to navigate to the previous page.
    /// </summary>
    public Uri? PreviousPage { get; set; }

    private static int EnsureValidPageNumber(int pageNumber) => pageNumber < 1 ? 1 : pageNumber;
    private static int EnsureValidPageSize(int pageSize) => pageSize > 1 ? pageSize : 1;
}