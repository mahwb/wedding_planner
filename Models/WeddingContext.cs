using Microsoft.EntityFrameworkCore;
 
namespace wedding_planner.Models
{
    public class WeddingContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public WeddingContext(DbContextOptions<WeddingContext> options) : base(options) { }

	// This DbSet contains "Person" objects and is called "Users"
    	public DbSet<User> Users { get; set; }
    	public DbSet<Wedding> Weddings { get; set; }
    	public DbSet<Guest> Guests { get; set; }
    }
}