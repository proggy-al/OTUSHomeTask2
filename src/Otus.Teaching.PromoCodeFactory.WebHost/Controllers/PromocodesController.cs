using System;
using System.Collections.Generic;
using System.Data;
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
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService;
        private readonly IPreferenceService _preferenceService;
        private readonly IEmployeeService _employeeService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public PromocodesController(IPromoCodeService promoCodeService,IPreferenceService preferenceService, 
            IEmployeeService employeeService, ICustomerService customerService,IMapper mapper)
        {
            _promoCodeService = promoCodeService;
            _preferenceService = preferenceService;
            _employeeService = employeeService;
            _customerService = customerService;
            _mapper = mapper;            
        }        

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promo = await _promoCodeService.GetAllPromoCodesAsync();
            var response = _mapper.Map<List<PromoCode>, List<PromoCodeShortResponse>>(promo.ToList());
            return Ok(response);
        }
        
        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            Preference preference = _preferenceService.GetAllPreferencesAsync().Result.Where(x => x.Name.Equals(request.Preference)).FirstOrDefault();
            Customer customers = _customerService.GetAllCustomersAsync().Result.ToList().
                Where(x => x.CustomerPreferences.Any(x=>x.PreferenceId == preference.Id)).FirstOrDefault(); 
            PromoCode newPromoCode = new PromoCode()
            {
                Id = Guid.NewGuid(),
                Code = request.PromoCode,
                ServiceInfo = request.ServiceInfo,
                PartnerName = request.PartnerName,
                Preference = preference,
                BeginDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(1),
                PartnerManager = _employeeService.GetAllEmployeesAsync().Result.FirstOrDefault(),
                Customer = customers
            };
            var response = await _promoCodeService.AddPromoCodeAsync(newPromoCode);
            return Ok($"New promocode is added with ID : {response.Id}");
        }
    }
}