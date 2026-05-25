using CarRentalConsole.Interfaces;
using CarRentalConsole.Views;

namespace CarRentalConsole.Helpers
{
    internal class ConsoleViewFactory
    {
        private readonly ICarService carService;
        private readonly IRentalService rentalService;

        public ConsoleViewFactory(ICarService carService, IRentalService rentalService)
        {
            this.carService = carService;
            this.rentalService = rentalService;
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
                case EMenuScreen.ReturnCar:
                    return new ReturnCarMenu(rentalService);
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
