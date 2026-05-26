using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;
using System.Text;

namespace CarRentalConsole.Views
{
    internal class RentCarMenu : IView
    {
        private readonly List<Car> availableCars;

        public RentCarMenu(List<Car> availableCars)
        {
            this.availableCars = availableCars;
        }

        public string Build()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string divider = new string('-', 40);

            if (availableCars.Count == 0)
            {
                stringBuilder.AppendLine("Sorry, there are no cars available for rent at the moment.");
            }
            else
            {
                stringBuilder.AppendLine("Renting a Car, please select your desired option:");

                for (int i = 0; i < availableCars.Count; i++)
                {
                    stringBuilder.AppendLine($"{availableCars[i].Id} - {availableCars[i].Name} ({availableCars[i].DailyRate} / day)");
                }
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
