using CarRentalConsole.Models;

namespace CarRentalConsole.Tests.Models
{
    [TestClass]
    public class RentalTest
    {
        [TestMethod]
        public void IsReturned_WhenReturnDateIsNull_ShouldReturnFalse()
        {
            Rental rental = new Rental
            {
                ReturnDate = null,
            };
            Assert.IsFalse(rental.IsReturned);
        }

        [TestMethod]
        public void IsReturned_WhenReturnDateHasValue_ShouldReturnTrue()
        {
            Rental rental = new Rental
            {
                ReturnDate = new DateOnly(2026, 5, 26),
            };
            Assert.IsTrue(rental.IsReturned);
        }
    }
}
