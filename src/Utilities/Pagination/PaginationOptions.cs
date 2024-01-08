namespace WebApi.Utilities.Pagination;

/// <summary>
/// Represents configuration settings for pagination.
/// Defines default and maximum values for page number and page size.
/// </summary>
public sealed class PaginationOptions
{
    /// <summary>
    /// Gets or sets the default page number. Used when no specific page number is specified.
    /// </summary>
    public int DefaultPageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the default size of each page. Used when no specific page size is specified.
    /// </summary>
    public int DefaultPageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the maximum permissible size of each page.
    /// This limit helps in preventing excessively large page requests.
    /// </summary>
    public int MaxPageSize { get; set; } = 50;
}