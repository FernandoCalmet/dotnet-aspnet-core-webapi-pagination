using Microsoft.AspNetCore.WebUtilities;

namespace WebApi.Utilities.Pagination;

/// <summary>
/// Provides functionality to create URIs for pagination.
/// </summary>
internal sealed class UriService : IUriService
{
    private readonly string _baseUri;
    private readonly IPaginationFilter _paginationFilter;

    /// <summary>
    /// Initializes a new instance of the <see cref="UriService"/> class.
    /// </summary>
    /// <param name="baseUri">The base URI to be used for generating URIs.</param>
    /// <param name="paginationFilter"></param>
    /// <exception cref="ArgumentNullException">Thrown when baseUri is null.</exception>
    public UriService(string baseUri, IPaginationFilter paginationFilter)
    {
        _baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
        _paginationFilter = paginationFilter;
    }

    /// <summary>
    /// Generates a URI based on the pagination filter and route.
    /// </summary>
    /// <param name="route">The route for which the URI is generated.</param>
    /// <returns>The generated URI.</returns>
    public Uri GetPageUri(string route)
    {
        var endpointUri = new Uri(string.Concat(_baseUri, route));
        var modifiedUri = AddQueryStringParameters(endpointUri);

        return new Uri(modifiedUri);
    }

    private string AddQueryStringParameters(Uri endpointUri)
    {
        var uriWithPageNumber = QueryHelpers.AddQueryString(
            endpointUri.ToString(),
            "pageNumber",
            _paginationFilter.PageNumber.ToString());

        return QueryHelpers.AddQueryString(
            uriWithPageNumber,
            "pageSize",
            _paginationFilter.PageSize.ToString());
    }
}