using CarRentalConsole.Models;

namespace CarRentalConsole.Interfaces
{
    internal interface IRentalService
    {
        Task<int> CreateRental(Rental rental);
        Task<Rental?> GetRentalById(int rentalId);
        Task<List<Rental>> GetOpenRentals();
        Task<ERentalReturnResult> ConcludeRental(int rentalId);
        Task<bool> HasOpenRentals();

    }
}
