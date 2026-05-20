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
                .Where(car => car.IsAvailable)
                .OrderBy(car => car.Id)
                .ToListAsync();
        }
    }
}
