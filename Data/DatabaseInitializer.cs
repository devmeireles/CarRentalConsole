using System;
using System.Collections.Generic;
using System.Text;

namespace CarRentalConsole.Data
{
    internal class DatabaseInitializer
    {
        private readonly AppDbContext dbContext;

        public DatabaseInitializer(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void initialize()
        {
            dbContext.Database.EnsureCreated();
        }
    }
}
