using System;
using System.Collections.Generic;
using System.Text;

namespace CarRentalConsole.Controllers
{
    internal class MenuController
    {
        private string ValidateInput(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return input.Trim();
        }

        private EMenuOption TransformMenuSelection(string input)
        {
            if (int.TryParse(input.Trim(), out int parsedInput))
            {
                return Enum.Parse<EMenuOption>(input);
            }

            throw new InvalidOperationException("The current state does not allow this operation.");

        }

        public void HandleSelection(EMenuOption currentMenu, string? input)
        {
            EMenuOption currentSelection = TransformMenuSelection(input);

            if (currentMenu == EMenuOption.Main)
            {
                Console.WriteLine($"selected {input} from {currentMenu}");
            }          


        }
    }
}
