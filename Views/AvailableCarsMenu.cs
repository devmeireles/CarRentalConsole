using CarRentalConsole.Interfaces;
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

        private string Build()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string divider = new string('-', 40);

            stringBuilder.AppendLine("Available Cars for Rent:");

            string[] availableCars = carService.GetAvailableCars();

            foreach (var car in availableCars)
            {
                stringBuilder.AppendLine(car);
            }

            stringBuilder.AppendLine(divider);

            return stringBuilder.ToString();
        }

        public void Display()
        {
            string menu = Build();
            Console.WriteLine(menu);
        }
    }
}
