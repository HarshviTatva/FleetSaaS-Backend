using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Entities;
using System;
namespace FleetSaaS.Application.Mapping
{
    public class DriverMappingProfile :Profile
    {
        public DriverMappingProfile()
        {
            CreateMap<DriverUserRequest, Driver>()
                .ForMember(x => x.LicenseNumber, opt => opt.MapFrom(src => src.LicenseNumber))
                .ForMember(x => x.LicenseExpiry, opt => opt.MapFrom(src => src.LicenseExpiryDate))
                .ForMember(x => x.IsAvailable, opt => opt.MapFrom(src=>src.IsAvailable))
                .ForMember(x => x.CompanyId, opt => opt.MapFrom(src => src.CompanyId));
        }
    }
}
