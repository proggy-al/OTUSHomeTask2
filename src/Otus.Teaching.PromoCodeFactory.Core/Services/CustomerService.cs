using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Services
{
    public class CustomerService(
        IRepository<Customer> customerRepository) : ICustomerService
    {

        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            var customer = await customerRepository.GetByIdAsync(id);
            return customer;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customers = await customerRepository.GetAllAsync();
            return customers;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            return await customerRepository.AddAsync(customer);
        }

        public async Task UpdateCustomer(Customer customer)
        {
            await customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteCustomer(Customer customer)
        {
            await customerRepository.DeleteAsync(customer);
        }       
    }
}
