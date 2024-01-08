namespace WebApi.Utilities.Pagination;

/// <summary>
/// Defines the interface for a pagination helper that creates paged responses.
/// </summary>
public interface IPaginationHelper
{
    /// <summary>
    /// Creates a paged response for a list of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the paged data.</typeparam>
    /// <param name="data">The data set to paginate.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The size of each page.</param>
    /// <param name="route">The base route used for generating URIs.</param>
    /// <returns>A paginated response containing the data and related pagination details.</returns>
    PagedResponse<IEnumerable<T>> CreatePagedResponse<T>(
        IEnumerable<T> data,
        int pageNumber,
        int pageSize,
        string route);
}