namespace WebApi.Contracts;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
}