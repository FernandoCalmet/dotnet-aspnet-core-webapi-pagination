using Microsoft.EntityFrameworkCore;
using WebApi.Contracts;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Services;

/// <summary>
/// Provides services to manage customers in the application.
/// </summary>
internal sealed class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerService"/> class.
    /// </summary>
    /// <param name="context">The application database context to be used in the service.</param>
    public CustomerService(ApplicationDbContext context) => _context = context;

    /// <summary>
    /// Retrieves a paginated list of customers.
    /// </summary>
    public async Task<IEnumerable<Customer>> GetCustomersAsync() =>
        await _context.Set<Customer>().ToListAsync();

    /// <summary>
    /// Retrieves a customer by their identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <returns>The customer if found, otherwise null.</returns>
    public async Task<Customer?> GetCustomerByIdAsync(Guid id) =>
        await _context.Set<Customer>().FirstOrDefaultAsync(customer => customer.Id == id);
}