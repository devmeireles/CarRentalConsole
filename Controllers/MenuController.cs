namespace CarRentalConsole.Controllers
{
    internal class MenuController
    {
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

        public EMenuScreen HandleSelection(EMenuScreen currentScreen, string? input)
        {
            return currentScreen switch
            {
                EMenuScreen.Main => HandleMainMenuSelection(input),
                _ => EMenuScreen.Main
            };
        }

    }
}
