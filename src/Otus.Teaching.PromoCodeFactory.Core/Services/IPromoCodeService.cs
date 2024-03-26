using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Services
{
    public interface IPromoCodeService
    {
        Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync();
        Task<PromoCode> AddPromoCodeAsync(PromoCode promoCode);
    }
}