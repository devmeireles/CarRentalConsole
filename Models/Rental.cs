using System.ComponentModel.DataAnnotations;

namespace CarRentalConsole.Models
{
    internal class Rental
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int CarId { get; set; }
        public Car? Car { get; set; }

        [Required]
        public int Duration { get; set; } = 0;
        [Required]
        public DateOnly StartDate { get; set; }
        [Required]
        public DateOnly EndDate { get; set; }

        [Required]
        public double TotalCost { get; set; }

    }
}
