using CarRentalConsole.Models;

namespace CarRentalConsole.Interfaces
{
    internal interface IRentalService
    {
        Task<int> CreateRental(Rental rental);
    }
}
