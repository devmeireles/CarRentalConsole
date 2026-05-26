using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;
using System.Text;

namespace CarRentalConsole.Views
{
    public class ReturnCarMenu : IView
    {
        private readonly List<Rental> openRentals;

        public ReturnCarMenu(List<Rental> openRentals)
        {
            this.openRentals = openRentals;
        }

        private string GetRentalDetails(Rental rental)
        {
            return $"Customer: {rental.Customer!.Email}, Car: {rental.Car!.Name}, rent from {rental.StartDate} to {rental.EndDate}. Total: {rental.TotalCost}";
        }

        private string Build()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string divider = new string('-', 40);

            if (openRentals.Count == 0)
            {
                stringBuilder.AppendLine("No cars are currently rented. Returning to main menu...");

            }
            else
            {
                stringBuilder.AppendLine("Returning a Car, please select your desired option:");
                for (int i = 0; i < openRentals.Count; i++)
                {
                    stringBuilder.AppendLine($"{openRentals[i].Id} - {this.GetRentalDetails(openRentals[i])}");
                }
                stringBuilder.AppendLine(divider);
                return stringBuilder.ToString();
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
