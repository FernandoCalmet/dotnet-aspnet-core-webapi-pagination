namespace WebApi.Utilities.Pagination;

/// <summary>
/// Represents a container for paginated data.
/// </summary>
/// <typeparam name="T">The type of data being paginated.</typeparam>
public sealed class PagedData<T>
{
    /// <summary>
    /// Gets or sets the list of paginated data.
    /// </summary>
    public List<T> Data { get; set; } = new();

    /// <summary>
    /// Gets or sets the total number of records.
    /// </summary>
    public int TotalRecords { get; set; }
}