using Bogus;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Utilities;
using WebApi.Utilities.Pagination;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerFakeController : ControllerBase
{
    private readonly IPaginationHelper _paginationHelper;

    public CustomerFakeController(IPaginationHelper paginationHelper) =>
        _paginationHelper = paginationHelper;

    [HttpGet]
    public IActionResult GetAll([FromQuery] PaginationFilterRequest filter)
    {
        var route = Request.Path.Value;

        if (route is null)
            return BadRequest(new CustomResponse<Customer>());

        var customers = CustomerFakeData.GetCustomers();

        var pagedResponse = _paginationHelper.CreatePagedResponse(
            customers,
            filter.PageNumber,
            filter.PageSize,
            route);

        return Ok(pagedResponse);
    }

    private static class CustomerFakeData
    {
        public static IEnumerable<Customer> GetCustomers()
        {
            var customers = new List<Customer>();
            var faker = new Faker();

            for (var i = 0; i < 100; i++)
            {
                var customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    Contact = faker.Phone.PhoneNumber(),
                    Email = faker.Internet.Email()
                };

                customers.Add(customer);
            }

            return customers;
        }
    }
}