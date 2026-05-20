using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;
using System.Text;

namespace CarRentalConsole.Views
{
    internal class RentCarMenu : IView
    {
        private readonly ICarService carService;

        public RentCarMenu(ICarService carService)
        {
            this.carService = carService;
        }

        public async Task<string> Build()
        {
            List<Car> availableCars = await carService.GetAvailableCars();

            StringBuilder stringBuilder = new StringBuilder();

            string divider = new string('-', 40);

            stringBuilder.AppendLine("Renting a Car, please select your desired option:");

            for (int i = 0; i < availableCars.Count; i++)
            {
                stringBuilder.AppendLine($"{availableCars[i].Id} - {availableCars[i].Name} ({availableCars[i].DailyRate} / day)");
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
