using CarRentalConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalConsole.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Car> Cars => Set<Car>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=car-rental.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().HasData(
                new Car { Id = 1, Name = "Toyota Camry", DailyRate = 120, IsAvailable = true },
                new Car { Id = 2, Name = "Honda Accord", DailyRate = 115, IsAvailable = true },
                new Car { Id = 3, Name = "Ford Mustang", DailyRate = 180, IsAvailable = true },
                new Car { Id = 4, Name = "Chevrolet Malibu", DailyRate = 105, IsAvailable = true },
                new Car { Id = 5, Name = "Nissan Altima", DailyRate = 110, IsAvailable = true },
                new Car { Id = 6, Name = "Hyundai Elantra", DailyRate = 90, IsAvailable = true },
                new Car { Id = 7, Name = "Kia Sportage", DailyRate = 130, IsAvailable = true },
                new Car { Id = 8, Name = "Jeep Wrangler", DailyRate = 170, IsAvailable = true },
                new Car { Id = 9, Name = "BMW 320i", DailyRate = 220, IsAvailable = true },
                new Car { Id = 10, Name = "Mercedes-Benz C300", DailyRate = 250, IsAvailable = true }
            );
        }
    }
}
