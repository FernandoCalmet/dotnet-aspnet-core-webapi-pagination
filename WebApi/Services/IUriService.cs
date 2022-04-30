namespace WebApi.Services;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
}