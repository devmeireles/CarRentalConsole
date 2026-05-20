using CarRentalConsole.Controllers;
using CarRentalConsole.Helpers;
using CarRentalConsole.Services;

namespace CarRentalConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CarService carService = new CarService();
            ConsoleInputReader inputReader = new ConsoleInputReader();

            CarRentalController carRentalController = new CarRentalController(carService, inputReader);

            ConsoleViewFactory consoleViewFactory = new ConsoleViewFactory(carService);
            MenuController menuController = new MenuController(carRentalController);

            EMenuScreen currentScreen = EMenuScreen.Main;

            while (currentScreen != EMenuScreen.Exit)
            {
                consoleViewFactory.DisplayView(currentScreen);

                string? menuSelection = Console.ReadLine();

                currentScreen = menuController.HandleSelection(currentScreen, menuSelection);

            }

        }
    }
}
