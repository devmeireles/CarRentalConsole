using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;
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

        private async Task<IView> GetView(EMenuScreen menuOption)
        {

            switch (menuOption)
            {
                case EMenuScreen.Main:
                    return new MainMenu();
                case EMenuScreen.AvailableCars:
                    List<Car> availableCars = await carService.GetAvailableCars();
                    return new AvailableCarsMenu(availableCars);
                case EMenuScreen.RentCar:
                    List<Car> rentableCars = await carService.GetAvailableCars();
                    return new RentCarMenu(rentableCars);
                case EMenuScreen.ReturnCar:
                    List<Rental> openRentals = await rentalService.GetOpenRentals();
                    return new ReturnCarMenu(openRentals);
                default:
                    return new NotFoundMenu();
            }
        }

        public async Task DisplayView(EMenuScreen menuOption)
        {
            IView view = await GetView(menuOption);
            view.Display();
        }
    }
}
