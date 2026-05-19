using CarRentalConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRentalConsole.Views
{
    internal class NotFoundMenu : IView
    {
        public void Display()
        {
            Console.WriteLine("Option Not Found - Please try again");
        }
    }
}
