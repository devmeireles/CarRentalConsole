using CarRentalConsole.Helpers;
using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;

namespace CarRentalConsole.Controllers
{
    internal class RentalController
    {
        private readonly ICarService carService;
        private readonly IRentalService rentalService;
        private readonly ICustomerService customerService;
        private readonly ConsoleInputReader inputReader;

        public RentalController(ICarService carService, IRentalService rentalService, ICustomerService customerService, ConsoleInputReader inputReader)
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

            Car? selectedCar = await carService.GetCarById(selectedOption);

            if (selectedCar is null)
            {
                Console.WriteLine("Invalid car selection. Please try again.");
                return EMenuScreen.RentCar;
            }

            if (!selectedCar.IsAvailable)
            {
                Console.WriteLine("Sorry, that car is no longer available.");
                return EMenuScreen.RentCar;
            }

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
            decimal totalCost = rentalDays * selectedCar.DailyRate;

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

            ERentalReturnResult result = await rentalService.ConcludeRental(rentalId);

            switch (result)
            {
                case ERentalReturnResult.Success:
                    Console.WriteLine("Car returned successfully.");
                    return EMenuScreen.Main;

                case ERentalReturnResult.RentalNotFound:
                    Console.WriteLine("No rental found with that ID.");
                    return EMenuScreen.ReturnCar;

                case ERentalReturnResult.AlreadyReturned:
                    Console.WriteLine("This car has already been returned.");
                    return EMenuScreen.ReturnCar;

                default:
                    Console.WriteLine("Failed to return car. Please try again.");
                    return EMenuScreen.ReturnCar;
            }
        }
    }
}
