namespace CarRentalConsole.Models
{
    public class Rental
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int CarId { get; set; }
        public Car? Car { get; set; }

        public int Duration { get; set; } = 0;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public decimal TotalCost { get; set; }

        public bool IsReturned => ReturnDate != null;
    }
}
