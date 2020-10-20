using Microsoft.EntityFrameworkCore;

namespace SharedTrip.Data
{
    public class ApplicationDbContext: DbContext
    {
        public static string ConnectionString = @"Server=.\SQLEXPRESS01;Database=SharedTrip;Integrated Security = True";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTrip>()
                .HasKey(ut => new { ut.TripId, ut.UserId });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserTrip> UserTrips { get; set; }
    }
}
