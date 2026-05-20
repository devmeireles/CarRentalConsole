using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;
using System.Text;

namespace CarRentalConsole.Views
{
    internal class AvailableCarsMenu : IView
    {

        private readonly ICarService carService;

        public AvailableCarsMenu(ICarService carService)
        {
            this.carService = carService;
        }

        private async Task<string> Build()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string divider = new string('-', 40);

            stringBuilder.AppendLine("Available Cars for Rent:");

            List<Car> availableCars = await carService.GetAvailableCars();

            foreach (var car in availableCars)
            {
                stringBuilder.AppendLine(car.Name);
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
