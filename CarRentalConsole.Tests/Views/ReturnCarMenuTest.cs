using CarRentalConsole.Models;
using CarRentalConsole.Views;

namespace CarRentalConsole.Tests.Views
{
    [DoNotParallelize]
    [TestClass]
    public class ReturnCarMenuTest
    {
        [TestMethod]
        public void Display_WhenNoOpenRentals_ShouldPrintNoCarsMessage()
        {
            TextWriter originalOutput = Console.Out;
            StringWriter writer = new StringWriter();
            try
            {
                List<Rental> openRentals = new List<Rental>();
                ReturnCarMenu menu = new ReturnCarMenu(openRentals);
                Console.SetOut(writer);
                menu.Display();
                string output = writer.ToString();
                StringAssert.Contains(output, "No cars are currently rented. Returning to main menu...");
            }
            finally
            {
                Console.SetOut(originalOutput);
                writer.Dispose();
            }
        }


        [TestMethod]
        public void Display_WhenOpenRentalsExist_ShouldPrintRentalDetails()
        {
            TextWriter originalOutput = Console.Out;
            StringWriter writer = new StringWriter();
            try
            {
                List<Rental> openRentals = new List<Rental>
                    {
                        new Rental
                        {
                            Id = 1,
                            Customer = new Customer { Email = "mail@mail.com" },
                            Car = new Car { Name = "Toyota Camry", DailyRate = 50, IsAvailable = true },
                            StartDate = new DateOnly(2026, 06, 01),
                            EndDate = new DateOnly(2026, 06, 10),
                            TotalCost = 500,
                        }
                    };

                ReturnCarMenu menu = new ReturnCarMenu(openRentals);
                Console.SetOut(writer);
                menu.Display();
                String output = writer.ToString();

                StringAssert.Contains(output, "Customer: mail@mail.com");
                StringAssert.Contains(output, "Car: Toyota Camry");
                StringAssert.Contains(output, "Total: 500");
            }
            finally
            {
                Console.SetOut(originalOutput);
                writer.Dispose();
            }
        }
    }
                
}
