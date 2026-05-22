
namespace CarRentalConsole.Controllers
{
    internal class MenuController
    {
        private readonly CarRentalController carRentalController;

        public MenuController(CarRentalController carRentalController)
        {
            this.carRentalController = carRentalController;
        }

        private TEnum ParseSelection<TEnum>(int input, out TEnum result) where TEnum : struct
        {

            result = (TEnum)Enum.ToObject(typeof(TEnum), input);

            return result;
        }

        private EMenuScreen HandleMainMenuSelection(string? input)
        {
            if (!int.TryParse(input, out int parsedInput))
            {
                return EMenuScreen.Main;
            }

            if (ParseSelection(parsedInput, out EMainMenuOption option) == default)
            {
                return EMenuScreen.Main;
            }

            return option switch
            {
                EMainMenuOption.ViewAvailableCars => EMenuScreen.AvailableCars,
                EMainMenuOption.RentCar => EMenuScreen.RentCar,
                EMainMenuOption.ReturnCar => EMenuScreen.ReturnCar,
                EMainMenuOption.Exit => EMenuScreen.Exit,
                _ => EMenuScreen.Main
            };
        }

        public async Task<EMenuScreen> HandleSelection(EMenuScreen currentScreen, string? input)
        {
            return currentScreen switch
            {
                EMenuScreen.Main => HandleMainMenuSelection(input),
                EMenuScreen.RentCar => await carRentalController.RentCar(input),
                _ => EMenuScreen.Main
            };
        }
    }
}
