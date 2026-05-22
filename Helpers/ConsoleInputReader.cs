namespace CarRentalConsole.Helpers
{
    internal class ConsoleInputReader
    {
        private bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        public DateOnly ReadDate(string message)
        {
            while (true)
            {
                Console.WriteLine(message);

                if (DateOnly.TryParse(Console.ReadLine(), out DateOnly date))
                {
                    return date;
                }

                Console.WriteLine("Invalid date format. Please enter a valid date");
            }
        }

        public string ReadEmail(string message)
        {
            while (true)
            {
                Console.WriteLine(message);

                string email = Console.ReadLine()?.Trim() ?? string.Empty;

                if (IsValidEmail(email))
                {
                    return email;
                }

                Console.WriteLine("Invalid email format. Please enter a valid email address.");
            }
        }
    }
}
