using CarRentalConsole.Controllers;
using CarRentalConsole.Factories;
using CarRentalConsole.Services;

namespace CarRentalConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CarService carService = new CarService();
            ConsoleViewFactory consoleViewFactory = new ConsoleViewFactory(carService);
            MenuController menuController = new MenuController(carService);

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
