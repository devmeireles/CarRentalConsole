using CarRentalConsole.Models;

namespace CarRentalConsole.Interfaces
{
    internal interface ICarService
    {
        Task<List<Car>> GetAvailableCars();
    }
}
