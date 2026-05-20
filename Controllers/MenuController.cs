
namespace CarRentalConsole.Controllers
{
    internal class MenuController
    {
        private readonly CarRentalController carRentalController;

        public MenuController(CarRentalController carRentalController)
        {
            this.carRentalController = carRentalController;
        }

        private bool TryParseSelection<TEnum>(string? input, out TEnum result) where TEnum : struct
        {
            result = default;

            if (!int.TryParse(input?.Trim(), out int parsedInput))
            {
                return false;
            }

            result = (TEnum)Enum.ToObject(typeof(TEnum), parsedInput);

            return true;
        }

        private EMenuScreen HandleMainMenuSelection(string? input)
        {
            if (!TryParseSelection(input, out EMainMenuOption option))
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
