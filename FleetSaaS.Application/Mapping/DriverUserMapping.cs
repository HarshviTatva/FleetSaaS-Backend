using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Mapping
{
    public class DriverUserMapping : Profile
    {
        public DriverUserMapping()
        {
            CreateMap<DriverUserRequest, User>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(src=>src.IsActive))
                .ForMember(x => x.CompanyId, opt => opt.MapFrom(src=>src.CompanyId));


        }
    }
}
