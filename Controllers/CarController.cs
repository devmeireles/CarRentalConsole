using CarRentalConsole.Helpers;
using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;

namespace CarRentalConsole.Controllers
{
    internal class CarController
    {
        private readonly ICarService carService;
        private readonly IRentalService rentalService;
        private readonly ICustomerService customerService;
        private readonly ConsoleInputReader inputReader;

        public CarController(ICarService carService, IRentalService rentalService, ICustomerService customerService, ConsoleInputReader inputReader)
        {
            this.carService = carService;
            this.inputReader = inputReader;
            this.rentalService = rentalService;
            this.customerService = customerService;
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

            string email = inputReader.ReadEmail("Enter your email: ");

            Customer? customer = await customerService.GetCustomerByEmail(email);

            if (customer == null)
            {
                Console.WriteLine(new string('-', 40));
                Console.WriteLine("No customer found with that email. Creating new one.");
                customer = await customerService.CreateCustomer(email);

                Console.WriteLine(new string('-', 40));
            }

            int rentalDays = endDate.DayNumber - startDate.DayNumber;
            double totalCost = rentalDays * (double)selectedCar.DailyRate;

            Rental newRent = new Rental
            {
                CarId = selectedCar.Id,
                CustomerId = customer.Id,
                StartDate = startDate,
                EndDate = endDate,
                Duration = rentalDays,
                TotalCost = totalCost,
            };

            int rentalId = await rentalService.CreateRental(newRent);

            if (rentalId > 0)
            {

                Console.WriteLine(new string('-', 40));

                Console.WriteLine($"Car selection {selectedCar.Name}");
                Console.WriteLine($"Rental days: {rentalDays}");
                Console.WriteLine($"Total cost: {totalCost:C}");
                Console.WriteLine($"Rental ID: {rentalId}");

                Console.WriteLine(new string('-', 40));

                return EMenuScreen.Main;
            }
            else
            {
                Console.WriteLine("Failed to create rental. Please try again.");
                return EMenuScreen.RentCar;
            }
        }

        public async Task<EMenuScreen> ReturnCar(string? input)
        {
            if (!int.TryParse(input?.Trim(), out int rentalId))
            {
                return EMenuScreen.ReturnCar;
            }

            Rental? rental = await rentalService.GetRentalById(rentalId);
            if (rental == null)
            {
                Console.WriteLine("No rental found with that ID.");
                return EMenuScreen.ReturnCar;
            }

            if (rental.IsReturned)
            {
                Console.WriteLine("This car has already been returned.");
                return EMenuScreen.ReturnCar;
            }

            int success = await rentalService.ConcludeRental(rentalId);
            if (success < 1)
            {
                Console.WriteLine("Failed to return car. Please try again.");
                return EMenuScreen.ReturnCar;
            }
            else
            {
                Console.WriteLine("Car returned successfully.");
                return EMenuScreen.Main;
            }
        }
    }
}
