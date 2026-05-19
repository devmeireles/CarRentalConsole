using CarRentalConsole.Interfaces;
using System.Text;

namespace CarRentalConsole.Views
{
    internal class AvailableCarsMenu : IView
    {
        private string[] availableCars = new string[]
        {
            "1. Toyota Camry",
            "2. Honda Accord",
            "3. Ford Mustang",
            "4. Tesla Model 3"
        };

        private string Build()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string divider = new string('-', 40);

            stringBuilder.AppendLine("Available Cars for Rent:");


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
