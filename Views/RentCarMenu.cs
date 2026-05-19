using CarRentalConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRentalConsole.Views
{
    internal class RentCarMenu : IView
    {
        public void Display()
        {
            Console.WriteLine("Rent a Car:");
        }
    }
}
