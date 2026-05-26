using CarRentalConsole.Interfaces;

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
