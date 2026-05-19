using CarRentalConsole.Interfaces;
using System.Text;

namespace CarRentalConsole.Views
{
    internal class MainMenu : IView
    {
        private string[] options = new string[]
        {
            "1. View Available Cars",
            "2. Rent a Car",
            "3. Return a Car",
            "4. Exit"
        };

        private string Build()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Welcome to the Car Rental System!");
            stringBuilder.AppendLine("Please select an option:");

            string divider = new string('-', 40);

            stringBuilder.AppendLine(divider);

            foreach (var option in options)
            {
                stringBuilder.AppendLine(option);
                if (option == options[^1])
                {
                    stringBuilder.AppendLine(divider);
                }
            }

            return stringBuilder.ToString();
        }

        public void Display()
        {
            string menu = Build();
            Console.WriteLine(menu);
        }
    }
}
