namespace CarRentalConsole.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal DailyRate { get; set; }
        public bool IsAvailable { get; set; } = true;

        public List<Rental> Rentals { get; set; } = new List<Rental>();

        public void MakeAvailable()
        {
            IsAvailable = true;
        }

        public void MakeUnavailable()
        {
            IsAvailable = false;
        }
    }
}
