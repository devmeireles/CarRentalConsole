using CarRentalConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRentalConsole.Services
{
    internal class CarService : ICarService
    {

        private string[] availableCars = new string[]
        {
            "Toyota Camry",
            "Honda Accord",
            "Ford Mustang",
            "Chevrolet Malibu",
            "Nissan Altima"
        };

        public string[] GetAvailableCars()
        {
            return availableCars;
        }
    }
}
