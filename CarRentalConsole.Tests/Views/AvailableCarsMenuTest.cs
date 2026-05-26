using CarRentalConsole.Models;
using CarRentalConsole.Views;


namespace CarRentalConsole.Tests.Views
{
    [DoNotParallelize]
    [TestClass]
    public class AvailableCarsMenuTest
    {
        [TestMethod]
        public void Display_WhenNoCarsExist_ShouldPrintNoCarsMessage()
        {
            TextWriter originalOutput = Console.Out;
            StringWriter writer = new StringWriter();
            try
            {
                List<Car> cars = new List<Car>();
                AvailableCarsMenu menu = new AvailableCarsMenu(cars);
                Console.SetOut(writer);
                menu.Display();
                string output = writer.ToString();
                StringAssert.Contains(output, "No cars are currently available for rent");
            }
            finally
            {
                Console.SetOut(originalOutput);
                writer.Dispose();
            }
        }

        [TestMethod]
        public void Display_WhenCarsExist_ShouldPrintCars()
        {
            TextWriter originalOutput = Console.Out;
            StringWriter writer = new StringWriter();
            try
            {
                List<Car> cars = new List<Car>
                {
                    new Car { Id = 1, Name = "Toyota Camry", DailyRate = 50, IsAvailable = true }
                };
                AvailableCarsMenu menu = new AvailableCarsMenu(cars);
                Console.SetOut(writer);
                menu.Display();
                string output = writer.ToString();
                StringAssert.Contains(output, "Available Cars for Rent:");
                StringAssert.Contains(output, "Toyota Camry");
            }
            finally
            {
                Console.SetOut(originalOutput);
                writer.Dispose();
            }
        }
    }
}
