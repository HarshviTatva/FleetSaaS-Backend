using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Mapping
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile() 
        {
            CreateMap<CompanyUserRegisterRequest, Company>()
                .ForMember(x => x.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(_=>true));
        }
    }
}
