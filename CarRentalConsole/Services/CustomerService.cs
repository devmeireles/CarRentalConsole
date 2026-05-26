using CarRentalConsole.Data;
using CarRentalConsole.Interfaces;
using CarRentalConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalConsole.Services
{
    internal class CustomerService : ICustomerService
    {
        private readonly AppDbContext dbContext;

        public CustomerService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Customer> CreateCustomer(string email)
        {
            string parsedEmail = email.Trim().ToLowerInvariant();

            Customer customer = new Customer
            {
                Email = parsedEmail
            };

            await dbContext.Customers.AddAsync(customer);

            await dbContext.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer?> GetCustomerByEmail(string email)
        {
            string parsedEmail = email.Trim().ToLowerInvariant();

            Customer? customer = await dbContext.Customers
                .FirstOrDefaultAsync(u => u.Email == parsedEmail);

            return customer;
        }
    }
}
