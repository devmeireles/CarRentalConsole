
using CarRentalConsole.Views;

namespace CarRentalConsole.Tests.Views
{
    [TestClass]
    public class MainMenuTest
    {
        [TestMethod]
        public void Display_ShouldPrintMenuOptions()
        {
            TextWriter originalOutput = Console.Out;
            StringWriter writer = new StringWriter();
            try
            {
                MainMenu menu = new MainMenu();
                Console.SetOut(writer);
                menu.Display();
                string output = writer.ToString();
                StringAssert.Contains(output, "Welcome to the Car Rental System!");
                StringAssert.Contains(output, "Please select an option:");
                StringAssert.Contains(output, "1. View Available Cars");
                StringAssert.Contains(output, "2. Rent a Car");
                StringAssert.Contains(output, "3. Return a Car");
                StringAssert.Contains(output, "4. Exit");
            }
            finally
            {
                Console.SetOut(originalOutput);
                writer.Dispose();
            }
        }
    }
}
