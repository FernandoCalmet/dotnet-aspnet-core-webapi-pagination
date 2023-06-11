using WebApi.Entities;

namespace WebApi.Contracts;

public interface ICustomerService
{
    Task<PagedData<Customer>> GetCustomersPage(PaginationFilter filter);
    Task<Customer?> GetCustomerById(Guid id);
}