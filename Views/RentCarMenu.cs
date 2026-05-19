using CarRentalConsole.Interfaces;
using System;
using System.Collections.Generic;
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

        public string Build()
        {
            string[] availableCars = carService.GetAvailableCars();

            StringBuilder stringBuilder = new StringBuilder();

            string divider = new string('-', 40);

            stringBuilder.AppendLine("Renting a Car, please select your desired option:");

            for (int i = 0; i < availableCars.Length; i++)
            {
                stringBuilder.AppendLine($"{i + 1} - {availableCars[i]}");
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
