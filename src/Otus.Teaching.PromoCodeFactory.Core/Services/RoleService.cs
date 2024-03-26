using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Services
{
    public class RoleService(IRepository<Role> roleRepository) : IRoleService
    {
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            var roles = await roleRepository.GetAllAsync();
            return roles;
        }
    }
}
