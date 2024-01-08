using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebApi.Utilities.Pagination;

/// <summary>
/// Provides extension methods for setting up pagination related services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds pagination-related services, including URI and pagination filter services, to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The application's configuration.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddPagination(this IServiceCollection services, IConfiguration configuration)
    {
        // Adds HttpContextAccessor as a singleton for accessing current HTTP context.
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Adds IUriService as a scoped dependency. This service is responsible for generating URIs based on the current request and pagination filters.
        services.AddScoped<IUriService>(o =>
        {
            var accessor = o.GetRequiredService<IHttpContextAccessor>();
            var request = accessor.HttpContext.Request;
            var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
            var paginationFilter = o.GetRequiredService<IPaginationFilter>();
            return new UriService(uri, paginationFilter);
        });

        // Configures PaginationOptions using the provided configuration section.
        services.Configure<PaginationOptions>(configuration.GetSection("Pagination"));

        // Registers IPaginationFilter as a scoped dependency, which represents the current request's pagination filters.
        services.AddScoped<IPaginationFilter, PaginationFilter>();

        // Registers IPaginationHelper as a scoped dependency, providing helper methods for creating paginated responses.
        services.AddScoped<IPaginationHelper, PaginationHelper>();

        return services;
    }
}