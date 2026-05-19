using CarRentalConsole.Interfaces;

namespace CarRentalConsole.Controllers
{
    internal class MenuController
    {

        private readonly ICarService carService;

        public MenuController(ICarService carService)
        {
            this.carService = carService;
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

        private EMenuScreen HandleRentCarMenuSelection(string? input)
        {
            if (!int.TryParse(input?.Trim(), out int selectedOption))
            {
                return EMenuScreen.RentCar;
            }

            if (selectedOption == 0)
            {
                return EMenuScreen.Main;
            }

            string[] availableCars = carService.GetAvailableCars();
            int carIndex = selectedOption - 1;

            if (carIndex < 0 || carIndex >= availableCars.Length)
            {
                return EMenuScreen.RentCar;
            }

            Console.WriteLine($"You selected car #{selectedOption}");

            return EMenuScreen.Main;
        }

        public EMenuScreen HandleSelection(EMenuScreen currentScreen, string? input)
        {
            return currentScreen switch
            {
                EMenuScreen.Main => HandleMainMenuSelection(input),
                EMenuScreen.RentCar => HandleRentCarMenuSelection(input),
                _ => EMenuScreen.Main
            };
        }
    }
}
