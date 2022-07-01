using Microsoft.EntityFrameworkCore;
using PassMoveAPI.Data.Entities;

namespace PassMoveAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
    }
}
