using Microsoft.EntityFrameworkCore;

namespace WebApi.Data.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Customer> Customers { get; set; }
}