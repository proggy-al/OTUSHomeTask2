using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Services
{
    public class PreferenceService(IRepository<Preference> preferenceRepository) : IPreferenceService
    {
        public async Task<IEnumerable<Preference>> GetAllPreferencesAsync()
        {
            var preferences = await preferenceRepository.GetAllAsync();
            return preferences;
        }
    }
}
