using WebApi.Entities;

namespace WebApi.Contracts;

/// <summary>
/// Defines the interface for a service that manages customers.
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Retrieves a paginated list of customers.
    /// </summary>
    Task<IEnumerable<Customer>> GetCustomersAsync();

    /// <summary>
    /// Retrieves a customer by their identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <returns>The customer if found, otherwise null.</returns>
    Task<Customer?> GetCustomerByIdAsync(Guid id);
}