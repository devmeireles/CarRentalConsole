using CarRentalConsole.Models;
using CarRentalConsole.Views;

namespace CarRentalConsole.Tests.Views
{
    [DoNotParallelize]
    [TestClass]
    public class RentCarMenuTests
    {
        [TestMethod]
        public void Display_WhenNoCarsExist_ShouldPrintNoCarsMessage()
        {
            TextWriter originalOutput = Console.Out;
            StringWriter writer = new StringWriter();

            try
            {
                List<Car> cars = new List<Car>();
                RentCarMenu menu = new RentCarMenu(cars);

                Console.SetOut(writer);

                menu.Display();

                string output = writer.ToString();

                StringAssert.Contains(output, "Sorry, there are no cars available for rent at the moment.");
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

                RentCarMenu menu = new RentCarMenu(cars);
                Console.SetOut(writer);

                menu.Display();

                string output = writer.ToString();

                StringAssert.Contains(output, "Toyota Camry");
                StringAssert.Contains(output, "50");
            }
            finally
            {
                Console.SetOut(originalOutput);
                writer.Dispose();
            }
        }
    }
}
