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
            RentalService rentService = new RentalService(dbContext);
            CarService carService = new CarService(dbContext);
            ConsoleInputReader inputReader = new ConsoleInputReader();

            CarRentalController carRentalController = new CarRentalController(carService, rentService, customerService, inputReader);

            ConsoleViewFactory consoleViewFactory = new ConsoleViewFactory(carService);
            MenuController menuController = new MenuController(carRentalController);

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
