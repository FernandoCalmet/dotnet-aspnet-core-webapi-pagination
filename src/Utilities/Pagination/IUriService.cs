namespace WebApi.Utilities.Pagination;

/// <summary>
/// Defines the interface for a service that generates URIs.
/// </summary>
public interface IUriService
{
    /// <summary>
    /// Generates a URI based on the pagination filter and a specific route.
    /// </summary>
    /// <param name="route">The route for which the URI is to be generated.</param>
    /// <returns>The generated URI.</returns>
    Uri GetPageUri(string route);
}