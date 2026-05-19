using CarRentalConsole.Controllers;
using CarRentalConsole.Services;

namespace CarRentalConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleViewFactory menuFactory = new ConsoleViewFactory();
            MenuController menuController = new MenuController();

            bool isRunning = true;
            EMenuOption currentMenu = EMenuOption.Main;

            while (isRunning)
            {
                menuFactory.DisplayView(currentMenu);

                string? menuSelection = Console.ReadLine();

                Console.WriteLine(menuSelection);

                menuController.HandleSelection(currentMenu, menuSelection);

            }

        }
    }
}
