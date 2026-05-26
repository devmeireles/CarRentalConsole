
using CarRentalConsole.Interfaces;

namespace CarRentalConsole.Controllers
{
    internal class MenuController
    {
        private readonly RentalController carRentalController;
        private readonly IRentalService rentalService;

        public MenuController(RentalController carRentalController, IRentalService rentalService)
        {
            this.carRentalController = carRentalController;
            this.rentalService = rentalService;
        }

        private TEnum ParseSelection<TEnum>(int input, out TEnum result) where TEnum : struct
        {

            result = (TEnum)Enum.ToObject(typeof(TEnum), input);

            return result;
        }

        private async Task<EMenuScreen> HandleMainMenuSelection(string? input)
        {
            if (!int.TryParse(input, out int parsedInput))
            {
                return EMenuScreen.Main;
            }

            if (ParseSelection(parsedInput, out EMainMenuOption option) == default)
            {
                return EMenuScreen.Main;
            }

            if (option == EMainMenuOption.ReturnCar)
            {
                bool hasOpenRentals = await rentalService.HasOpenRentals();

                if (!hasOpenRentals)
                {
                    Console.WriteLine("No cars are currently rented.");
                    return EMenuScreen.Main;
                }

                return EMenuScreen.ReturnCar;
            }

            return option switch
            {
                EMainMenuOption.ViewAvailableCars => EMenuScreen.AvailableCars,
                EMainMenuOption.RentCar => EMenuScreen.RentCar,
                EMainMenuOption.Exit => EMenuScreen.Exit,
                _ => EMenuScreen.Main
            };
        }

        public async Task<EMenuScreen> HandleSelection(EMenuScreen currentScreen, string? input)
        {
            return currentScreen switch
            {
                EMenuScreen.Main => await HandleMainMenuSelection(input),
                EMenuScreen.RentCar => await carRentalController.RentCar(input),
                EMenuScreen.ReturnCar => await carRentalController.ReturnCar(input),
                _ => EMenuScreen.Main
            };
        }
    }
}
