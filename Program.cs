using CarRentalConsole.Controllers;
using CarRentalConsole.Factories;

namespace CarRentalConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleViewFactory menuFactory = new ConsoleViewFactory();
            MenuController menuController = new MenuController();

            EMenuScreen currentScreen = EMenuScreen.Main;

            while (currentScreen != EMenuScreen.Exit)
            {
                menuFactory.DisplayView(currentScreen);

                string? menuSelection = Console.ReadLine();

                Console.WriteLine(menuSelection);

                currentScreen = menuController.HandleSelection(currentScreen, menuSelection);

            }

        }
    }
}
