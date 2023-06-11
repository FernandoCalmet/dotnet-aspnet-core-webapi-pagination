using Microsoft.EntityFrameworkCore;
using WebApi.Contracts;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Services;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;

    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedData<Customer>> GetCustomersPage(PaginationFilter filter)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var pagedData = await _context.Customers
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();
        var totalRecords = await _context.Customers.CountAsync();

        return new PagedData<Customer>
        {
            Data = pagedData,
            TotalRecords = totalRecords
        };
    }

    public async Task<Customer?> GetCustomerById(Guid id)
    {
        return await _context.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
    }
}