using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Services;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _emloyeeService;
        private readonly IMapper _mapper;
        public EmployeesController(IEmployeeService emloyeeService, IMapper mapper)
        {
            _emloyeeService = emloyeeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _emloyeeService.GetAllEmployeesAsync();
            var response = _mapper.Map<List<Employee>, List<EmployeeShortResponse>>(employees.ToList());
            return Ok(response);
        }
        
        /// <summary>
        /// Получить данные сотрудника по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _emloyeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();            
            var response = _mapper.Map<Employee, EmployeeResponse>(employee);
            return Ok(response);
        }
    }
}