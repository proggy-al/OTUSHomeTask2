using AutoMapper;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System.Linq;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mapping
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<Customer, CustomerShortResponse>();
            CreateMap<Role, RoleItemResponse>();
            CreateMap<Employee, EmployeeShortResponse>();
            CreateMap<Employee, EmployeeResponse>();
            CreateMap<Preference, PreferenceResponse>();
            CreateMap<Customer, CustomerResponse>()
                .ForMember(b => b.Preference, map => map.MapFrom(x=>x.CustomerPreferences
                .Select(z=> new PreferenceResponse()
                {
                    Id= z.Id,
                    Name = z.Preference.Name
                }).ToList()))
                .ForMember(f => f.PromoCodes, map => map.MapFrom(x=>x.PromoCodes
                .Select(z=> new PromoCodeShortResponse()
                {
                    Id = z.Id,
                    Code = z.Code,
                    ServiceInfo = z.ServiceInfo,
                    BeginDate = z.BeginDate.ToShortDateString(),
                    EndDate = z.EndDate.ToShortDateString(),
                    PartnerName = z.PartnerName
                }).ToList()));

            CreateMap<PromoCode, PromoCodeShortResponse>();
        }
    }
}
