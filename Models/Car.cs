using System;
using System.Collections.Generic;
using System.Text;

namespace CarRentalConsole.Models
{
    internal class Car
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal DailyRate { get; set; }
        public bool IsAvailable { get; set; } = true;

    }
}
