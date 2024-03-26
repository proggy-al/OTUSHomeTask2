using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Services
{
    public class PromoCodeService(IRepository<PromoCode> promocodeRepository) : IPromoCodeService
    {
        public async Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync()
        {
            var promoCodes = await promocodeRepository.GetAllAsync();
            return promoCodes;
        }

        public async Task<PromoCode> AddPromoCodeAsync(PromoCode promoCode)
        {
            return await promocodeRepository.AddAsync(promoCode);
        }
    }
}
