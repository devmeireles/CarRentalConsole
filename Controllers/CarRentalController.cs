using CarRentalConsole.Helpers;
using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;

namespace CarRentalConsole.Controllers
{
    internal class CarRentalController
    {
        private readonly ICarService carService;
        private readonly ConsoleInputReader inputReader;

        public CarRentalController(ICarService carService, ConsoleInputReader inputReader)
        {
            this.carService = carService;
            this.inputReader = inputReader;
        }

        public async Task<EMenuScreen> RentCar(string? input)
        {
            if (!int.TryParse(input?.Trim(), out int selectedOption))
            {
                return EMenuScreen.RentCar;
            }

            List<Car> availableCars = await carService.GetAvailableCars();

            int carIndex = selectedOption - 1;

            if (carIndex < 0 || carIndex >= availableCars.Count)
            {
                Console.WriteLine("Invalid car option.");
                return EMenuScreen.RentCar;
            }

            Car selectedCar = availableCars[carIndex];

            DateOnly startDate = inputReader.ReadDate("Start date: ");
            DateOnly endDate = inputReader.ReadDate("End date: ");

            if (endDate <= startDate)
            {
                Console.WriteLine("End date must be after start date.");
                return EMenuScreen.RentCar;
            }

            int rentalDays = endDate.DayNumber - startDate.DayNumber;

            Console.WriteLine($"You selected {selectedCar.Name}");
            Console.WriteLine($"Rental days: {rentalDays}");

            return EMenuScreen.Main;
        }

    }
}
