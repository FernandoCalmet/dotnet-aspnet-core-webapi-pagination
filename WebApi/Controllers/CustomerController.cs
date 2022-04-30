using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data.Contexts;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IUriService uriService;
    public CustomerController(ApplicationDbContext context, IUriService uriService)
    {
        this.context = context;
        this.uriService = uriService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var pagedData = await context.Customers
           .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
           .Take(validFilter.PageSize)
           .ToListAsync();
        var totalRecords = await context.Customers.CountAsync();
        var pagedReponse = PaginationHelper.CreatePagedReponse<Customer>(pagedData, validFilter, totalRecords, uriService, route);
        return Ok(pagedReponse);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await context.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
        return Ok(new Response<Customer>(customer));
    }
}