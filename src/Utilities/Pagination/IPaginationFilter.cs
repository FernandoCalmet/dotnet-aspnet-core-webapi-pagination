namespace WebApi.Utilities.Pagination;

public interface IPaginationFilter
{
    /// <summary>
    /// Gets or sets the page number. The value is always greater than or equal to 1.
    /// </summary>
    int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the page size. The value is always greater than or equal to 1 and less than or equal to the maximum page size.
    /// </summary>
    int PageSize { get; set; }
}