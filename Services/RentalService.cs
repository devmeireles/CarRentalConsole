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
                .Include(rental => rental.Car)
                .Include(rental => rental.Customer)
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
            return await dbContext.Rentals
                .Include(rental => rental.Car)
                .Include(rental => rental.Customer)
                .FirstOrDefaultAsync(rental => rental.Id == rentalId);
        }

        public async Task<List<Car>> GetRentedCars()
        {
            return await dbContext.Cars
                .Where(car => !car.IsAvailable)
                .OrderBy(car => car.Id)
                .ToListAsync();
        }

        public async Task<ERentalReturnResult> ConcludeRental(int rentalId)
        {
            Rental? rental = await dbContext.Rentals
                .Include(rental => rental.Car)
                .FirstOrDefaultAsync(rental => rental.Id == rentalId);

            if (rental == null)
            {
                return ERentalReturnResult.RentalNotFound;
            }

            if (rental.IsReturned)
            {
                return ERentalReturnResult.AlreadyReturned;
            }

            rental.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
            rental.Car!.MakeAvailable();

            await dbContext.SaveChangesAsync();

            return ERentalReturnResult.Success;
        }
    }
}
