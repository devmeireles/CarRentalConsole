using CarRentalConsole.Models;

namespace CarRentalConsole.Interfaces
{
    internal interface IRentalService
    {
        Task<int> CreateRental(Rental rental);
        Task<Rental?> GetRentalById(int rentalId);
        Task<string> GetRentalDetails(int rentalId);
        Task<List<Rental>> GetOpenRentals();
        Task<int> ConcludeRental(int carId);

    }
}
