using Gbc_Travel_Group63.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Gbc_Travel_Group63.Data
{
    
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Flights> Flights { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }

        public DbSet<ProfileViewModel> ProfileViewModel { get; set; }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                // Customize your identity model if needed
                // For example, you can configure additional properties or relationships here
            }
    }
    
}
