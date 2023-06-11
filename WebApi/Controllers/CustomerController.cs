using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;
using WebApi.Entities;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IUriService _uriService;
    private readonly IPaginationHelper _paginationHelper;

    public CustomerController(ICustomerService customerService, IUriService uriService, IPaginationHelper paginationHelper)
    {
        this._customerService = customerService;
        this._uriService = uriService;
        _paginationHelper = paginationHelper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
    {
        var route = Request.Path.Value;

        if (route is null)
            return BadRequest(new Response<Customer>());

        var pagedData = await _customerService.GetCustomersPage(filter);
        var pagedResponse = _paginationHelper.CreatePagedResponse(pagedData.Data, filter, pagedData.TotalRecords, _uriService, route);
        return Ok(pagedResponse);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _customerService.GetCustomerById(id);

        if (customer == null)
            return NotFound();

        return Ok(new Response<Customer>(customer));
    }
}