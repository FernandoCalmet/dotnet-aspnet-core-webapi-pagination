using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Customer> Customers { get; set; } = null!;
}