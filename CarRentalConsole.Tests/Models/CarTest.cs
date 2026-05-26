using CarRentalConsole.Models;

namespace CarRentalConsole.Tests.Models
{
    [TestClass]
    public class CarTest
    {
        [TestMethod]
        public void MakeUnavailable_ShouldSetIsAvailableToFalse()
        {
            Car car = new Car
            {
                IsAvailable = false,
            };

            car.MakeUnavailable();

            Assert.IsFalse(car.IsAvailable);
        }

        [TestMethod]
        public void MakeAvailable_ShouldSetIsAvailableToTrue()
        {
            Car car = new Car
            {
                IsAvailable = false,
            };
            car.MakeAvailable();
            Assert.IsTrue(car.IsAvailable);
        }
    }
}
