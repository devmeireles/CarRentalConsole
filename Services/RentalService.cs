using CarRentalConsole.Data;
using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalConsole.Services
{
    internal class RentalService : IRentalService
    {
        private readonly AppDbContext dbContext;

        public RentalService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Rental>> GetOpenRentals()
        {
            return await dbContext.Rentals
                .Where(r => r.ReturnDate == null)
                .ToListAsync();
        }

        public async Task<int> CreateRental(Rental rental)
        {
            Car? car = await dbContext.Cars.FindAsync(rental.CarId);

            if (car == null)
            {
                return 0;
            }

            car.IsAvailable = false;


            await dbContext.Rentals.AddAsync(rental);
            int affectedRows = await dbContext.SaveChangesAsync();

            return rental.Id;
        }

        public async Task<Rental?> GetRentalById(int rentalId)
        {
            return await dbContext.Rentals.FindAsync(rentalId);
        }

        public async Task<List<Car>> GetRentedCars()
        {
            return await dbContext.Cars
                .Where(car => !car.IsAvailable)
                .OrderBy(car => car.Id)
                .ToListAsync();
        }

        public async Task<int> ConcludeRental(int rentalId)
        {
            Rental? rental = await dbContext.Rentals
                .Include(rental => rental.Car)
                .FirstOrDefaultAsync(rental => rental.Id == rentalId);

            if (rental == null)
            {
                return 0;
            }

            rental.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
            rental.Car!.MakeAvailable();

            return await dbContext.SaveChangesAsync();
        }

        public async Task<string> GetRentalDetails(int rentalId)
        {
            var details = await dbContext.Rentals
                .Where(rental => rental.Id == rentalId)
                .Select(rental => new
                {

                    CustomerEmail = rental.Customer!.Email,
                    CarName = rental.Car!.Name,
                    StartDate = rental.StartDate,
                    EndDate = rental.EndDate,
                    TotalCost = rental.TotalCost


                })
                .FirstOrDefaultAsync();

            if (details is null)
            {
                return "Rental not found.";
            }

            return $"Customer: {details.CustomerEmail}, Car: {details.CarName}, rent from {details.StartDate} to {details.EndDate}. Total: {details.TotalCost:F2}";
        }
    }
}
