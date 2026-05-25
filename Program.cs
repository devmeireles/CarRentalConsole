using CarRentalConsole.Controllers;
using CarRentalConsole.Data;
using CarRentalConsole.Helpers;
using CarRentalConsole.Services;

namespace CarRentalConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AppDbContext dbContext = new AppDbContext();
            DatabaseInitializer databaseInitializer = new DatabaseInitializer(dbContext);
            databaseInitializer.initialize();

            CustomerService customerService = new CustomerService(dbContext);
            RentalService rentalService = new RentalService(dbContext);
            CarService carService = new CarService(dbContext);
            ConsoleInputReader inputReader = new ConsoleInputReader();

            CarController carController = new CarController(carService, rentalService, customerService, inputReader);

            ConsoleViewFactory consoleViewFactory = new ConsoleViewFactory(carService, rentalService);
            MenuController menuController = new MenuController(carController);

            EMenuScreen currentScreen = EMenuScreen.Main;

            while (currentScreen != EMenuScreen.Exit)
            {
                consoleViewFactory.DisplayView(currentScreen);

                string? menuSelection = Console.ReadLine();

                currentScreen = await menuController.HandleSelection(currentScreen, menuSelection);

            }

        }
    }
}
