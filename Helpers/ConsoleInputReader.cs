namespace CarRentalConsole.Helpers
{
    internal class ConsoleInputReader
    {

        public DateOnly ReadDate(string message)
        {
            while (true)
            {
                Console.Write(message);

                if(DateOnly.TryParse(Console.ReadLine(), out DateOnly date))
                {
                    return date;
                }

                Console.WriteLine("Invalid date format. Please enter a valid date");
            }
        }
    }
}
