using Microsoft.AspNetCore.WebUtilities;
using WebApi.Contracts;

namespace WebApi.Services;

public class UriService : IUriService
{
    private readonly string _baseUri;

    public UriService(string baseUri)
    {
        _baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
    }

    public Uri GetPageUri(PaginationFilter filter, string route)
    {
        var endpointUri = new Uri(string.Concat(_baseUri, route));
        var modifiedUri = AddQueryStringParameters(endpointUri, filter);

        return new Uri(modifiedUri);
    }

    private string AddQueryStringParameters(Uri endpointUri, PaginationFilter filter)
    {
        var uriWithPageNumber = QueryHelpers.AddQueryString(endpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
        return QueryHelpers.AddQueryString(uriWithPageNumber, "pageSize", filter.PageSize.ToString());
    }
}