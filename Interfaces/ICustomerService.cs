using CarRentalConsole.Models;

namespace CarRentalConsole.Interfaces
{
    internal interface ICustomerService
    {
        Task<Customer?> GetCustomerByEmail(string email);
        Task<Customer> CreateCustomer(string email);
    }
}
