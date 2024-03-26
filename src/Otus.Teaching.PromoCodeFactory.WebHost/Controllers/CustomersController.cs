using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Core.Services;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase        
    {
        private readonly ICustomerService _customerService;        
        private readonly IMapper _mapper;

        public CustomersController(ICustomerService customerService, IPreferenceService preferenceService, IMapper mapper)
        {
            _customerService = customerService;            
            _mapper = mapper;            
        }

        /// <summary>
        /// Получение списка клиентов (без предпочтений)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            var response = _mapper.Map<List<Customer>,List<CustomerShortResponse>>(customers.ToList());
            return Ok(response);
        }
        
        /// <summary>
        /// Получение клиента с выданными ему промокодами и предпочтениями
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            Customer customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<Customer, CustomerResponse>(customer);
            return Ok(response);
        }
        
        /// <summary>
        /// Добавление клиента и его предпочтения
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            Customer newCustomer = new Customer()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                CustomerPreferences = request.PreferenceIds.Select(x => new CustomerPreference()
                {
                    Id = Guid.NewGuid(),
                    PreferenceId = x
                }).ToList()                
            };
            var response = await _customerService.AddCustomerAsync(newCustomer);             
            return Ok($"New customer is added with ID : {response.Id}");
        }

        /// <summary>
        /// Обновить данные клиента и его предпочтения
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            Customer customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            List<CustomerPreference> p = new List<CustomerPreference>();
            // текущие предпочтения
            List<Guid> currentCustomerPreferences = customer.CustomerPreferences.Select(x => x.PreferenceId).ToList();
            foreach (Guid x in request.PreferenceIds)
            {
                // добавляем новое предпочтение
                if (!currentCustomerPreferences.Contains(x))
                {
                    p.Add(new CustomerPreference()
                    {
                        CustomerId = id,
                        PreferenceId = x,
                        Id = Guid.NewGuid()
                    });
                }
                else
                {
                    // оставляем которое уже было
                    p.Add(new CustomerPreference()
                    {
                        CustomerId = id,
                        PreferenceId = x,
                        Id = customer.CustomerPreferences.Where(z=>z.PreferenceId==x && z.CustomerId==id).Select(z => z.Id).FirstOrDefault()
                    });
                }
            }        
            customer.CustomerPreferences = p;
            await _customerService.UpdateCustomer(customer);
            return Ok($"Data customer is updated for ID : {id}");
        }

        /// <summary>
        /// Удалить клиента и его промокоды и его предпочтения
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            Customer customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) 
                return NotFound();
            await _customerService.DeleteCustomer(customer);
            return Ok("Customer is deleted");
        }
    }
}