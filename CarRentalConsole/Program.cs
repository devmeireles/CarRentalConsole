using CarRentalConsole.Controllers;
using CarRentalConsole.Data;
using CarRentalConsole.Helpers;
using CarRentalConsole.Interfaces;
using CarRentalConsole.Services;

namespace CarRentalConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AppDbContext dbContext = new AppDbContext();

            CustomerService customerService = new CustomerService(dbContext);
            RentalService rentalService = new RentalService(dbContext);
            CarService carService = new CarService(dbContext);
            ConsoleInputReader inputReader = new ConsoleInputReader();

            RentalController carRentalController = new RentalController(carService, rentalService, customerService, inputReader);

            ConsoleViewFactory consoleViewFactory = new ConsoleViewFactory(carService, rentalService);
            MenuController menuController = new MenuController(carRentalController, rentalService);

            EMenuScreen currentScreen = EMenuScreen.Main;

            while (currentScreen != EMenuScreen.Exit)
            {
                await consoleViewFactory.DisplayView(currentScreen);

                string? menuSelection = Console.ReadLine();

                currentScreen = await menuController.HandleSelection(currentScreen, menuSelection);
            }

        }
    }
}
