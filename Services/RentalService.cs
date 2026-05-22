using CarRentalConsole.Data;
using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;

namespace CarRentalConsole.Services
{
    internal class RentalService : IRentalService
    {
        private readonly AppDbContext dbContext;

        public RentalService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
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
    }
}
