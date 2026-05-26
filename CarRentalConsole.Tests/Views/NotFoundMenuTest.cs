using CarRentalConsole.Views;

namespace CarRentalConsole.Tests.Views
{
    [DoNotParallelize]
    [TestClass]
    public class NotFoundMenuTest
    {
        [TestMethod]
        public void Display_ShouldPrintNotFoundMessage()
        {
            TextWriter originalOutput = Console.Out;
            StringWriter writer = new StringWriter();
            try
            {
                NotFoundMenu menu = new NotFoundMenu();
                Console.SetOut(writer);
                menu.Display();
                string output = writer.ToString();
                StringAssert.Contains(output, "Option Not Found - Please try again");
            }
            finally
            {
                Console.SetOut(originalOutput);
                writer.Dispose();
            }
        }
    }
}
