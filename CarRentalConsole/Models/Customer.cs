using System.ComponentModel.DataAnnotations;

namespace CarRentalConsole.Models
{    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;

        public List<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
