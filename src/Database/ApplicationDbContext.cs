using Clean.Solutions.Vertical.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clean.Solutions.Vertical.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
    public DbSet<Todo> Todos { get; set; }
}
