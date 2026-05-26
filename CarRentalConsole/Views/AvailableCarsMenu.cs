using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;
using System.Text;

namespace CarRentalConsole.Views
{
    public class AvailableCarsMenu : IView
    {

        private readonly List<Car> availableCars;

        public AvailableCarsMenu(List<Car> availableCars)
        {
            this.availableCars = availableCars;
        }

        private string Build()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string divider = new string('-', 40);

            if (availableCars.Count == 0)
            {
                stringBuilder.AppendLine("No cars are currently available for rent.");
                stringBuilder.AppendLine(divider);
                return stringBuilder.ToString();
            }

            stringBuilder.AppendLine("Available Cars for Rent:");

            foreach (var car in availableCars)
            {
                stringBuilder.AppendLine(car.Name);
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
