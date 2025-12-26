using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<CompanyUserRegisterRequest, User>()
                .ForMember(x=>x.UserName,opt=>opt.MapFrom(src=>src.OwnerName))
                .ForMember(x=>x.Email,opt=>opt.MapFrom(src=>src.OwnerEmail))
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(src => src.OwnerPhoneNumber))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(x => x.CompanyId, opt => opt.Ignore());
        }
    }
}
