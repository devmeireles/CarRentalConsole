using CarRentalConsole.Interfaces;
using CarRentalConsole.Views;

namespace CarRentalConsole.Helpers
{
    internal class ConsoleViewFactory
    {
        private readonly ICarService carService;

        public ConsoleViewFactory(ICarService carService)
        {
            this.carService = carService;
        }

        private IView GetView(EMenuScreen menuOption)
        {

            switch (menuOption)
            {
                case EMenuScreen.Main:
                    return new MainMenu();
                case EMenuScreen.AvailableCars:
                    return new AvailableCarsMenu(carService);
                case EMenuScreen.RentCar:
                    return new RentCarMenu(carService);
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
