using Microsoft.EntityFrameworkCore;
using wema_swe.Models;


namespace wema_swe.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }  
        public DbSet<Otp> Otps { get; set; }
    }
}
