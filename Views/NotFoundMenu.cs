using CarRentalConsole.Interfaces;

namespace CarRentalConsole.Views
{
    internal class NotFoundMenu : IView
    {
        public async Task Display()
        {
            Console.WriteLine("Option Not Found - Please try again");
        }
    }
}
