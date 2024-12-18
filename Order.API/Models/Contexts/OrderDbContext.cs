using Microsoft.EntityFrameworkCore;
using Order.API.Models.Entites;

namespace Order.API.Models.Contexts
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Models.Entites.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderOutbox> OrderOutboxes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderOutbox>().HasNoKey();
        }

    }
}
