using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Core.Services;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController : ControllerBase 
    {
        private readonly IPreferenceService _preferenceService;
        private readonly IMapper _mapper;

        public PreferencesController(IPreferenceService preferenceService, IMapper mapper)
        {
            _preferenceService = preferenceService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все предпочтения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PreferenceResponse>> GetPreferencesAsync()
        {
            var preferences = await _preferenceService.GetAllPreferencesAsync();
            var response = _mapper.Map<List<Preference>, List<PreferenceResponse>>(preferences.ToList());
            return Ok(response);
        }
    }
}
