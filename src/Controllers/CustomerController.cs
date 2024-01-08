using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;
using WebApi.Entities;
using WebApi.Utilities;
using WebApi.Utilities.Pagination;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IPaginationHelper _paginationHelper;

    public CustomerController(ICustomerService customerService, IPaginationHelper paginationHelper)
    {
        _customerService = customerService;
        _paginationHelper = paginationHelper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationFilterRequest filter)
    {
        var route = Request.Path.Value;

        if (route is null)
            return BadRequest(new CustomResponse<Customer>());

        var customers = await _customerService.GetCustomersAsync();

        var pagedResponse = _paginationHelper.CreatePagedResponse(
            customers,
            filter.PageNumber,
            filter.PageSize,
            route);

        return Ok(pagedResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);

        if (customer == null)
            return NotFound();

        return Ok(new CustomResponse<Customer>(customer));
    }
}