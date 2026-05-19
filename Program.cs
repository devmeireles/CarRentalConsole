using CarRentalConsole.Services;
using CarRentalConsole.Views;

namespace CarRentalConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleViewFactory menuFactory = new ConsoleViewFactory();

            menuFactory.DisplayView(EMenuOption.Main);

        }
    }
}
