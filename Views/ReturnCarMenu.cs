using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;
using System.Text;

namespace CarRentalConsole.Views
{
    internal class ReturnCarMenu : IView
    {
        private readonly IRentalService rentalService;

        public ReturnCarMenu(IRentalService rentalService)
        {
            this.rentalService = rentalService;
        }

        private async Task<string> Build()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string divider = new string('-', 40);

            List<Rental> openRentals = await rentalService.GetOpenRentals();

            if (openRentals.Count == 0)
            {
                stringBuilder.AppendLine("No cars are currently rented. Returning to main menu...");

            }
            else
            {
                stringBuilder.AppendLine("Returning a Car, please select your desired option:");
                for (int i = 0; i < openRentals.Count; i++)
                {
                    stringBuilder.AppendLine($"{openRentals[i].Id} - {await rentalService.GetRentalDetails(openRentals[i].Id)}");
                }
                stringBuilder.AppendLine(divider);
                return stringBuilder.ToString();
            }

            stringBuilder.AppendLine(divider);

            return stringBuilder.ToString();
        }

        public async Task Display()
        {
            string menu = await Build();
            Console.WriteLine(menu);
        }
    }
}
