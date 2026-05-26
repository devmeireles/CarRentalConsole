using CarRentalConsole.Data;
using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalConsole.Services
{
    internal class CarService : ICarService
    {
        private readonly AppDbContext dbContext;

        public CarService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Car>> GetAvailableCars()
        {
            return await dbContext.Cars
                .AsNoTracking()
                .Where(car => car.IsAvailable)
                .OrderBy(car => car.Id)
                .ToListAsync();
        }

        public async Task<Car?> GetCarById(int carId)
        {
            return await dbContext.Cars
                .FirstOrDefaultAsync(car => car.Id == carId);
        }

        public Task<bool> IsCarAvailable(int carId)
        {
            return dbContext.Cars
                .Where(car => car.Id == carId)
                .Select(car => car.IsAvailable)
                .FirstOrDefaultAsync();
        }
    }
}
