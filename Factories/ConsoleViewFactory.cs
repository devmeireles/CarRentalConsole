using CarRentalConsole.Interfaces;
using CarRentalConsole.Views;

namespace CarRentalConsole.Factories
{
    internal class ConsoleViewFactory
    {
        private IView GetView(EMenuScreen menuOption)
        {

            switch (menuOption)
            {
                case EMenuScreen.Main:
                    return new MainMenu();
                case EMenuScreen.AvailableCars:
                    return new AvailableCarsMenu();
                case EMenuScreen.RentCar:
                    return new RentCarMenu();
                default:
                    return new NotFoundMenu();
            }
        }

        public void DisplayView(EMenuScreen menuOption)
        {
            IView view = GetView(menuOption);
            view.Display();
        }
    }
}
