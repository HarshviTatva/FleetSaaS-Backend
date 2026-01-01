using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Mapping
{
    public class TripMappingProfile : Profile
    {
        public TripMappingProfile() 
        {
            CreateMap<TripRequest, Trip>()
                .ForMember(x => x.Origin, opt => opt.MapFrom(src => src.Origin))
                .ForMember(x => x.Destination, opt => opt.MapFrom(src => src.Destination))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                 .ForMember(d => d.ScheduledAt, opt => opt.MapFrom(src => src.ScheduledAt.HasValue
                ? DateTime.SpecifyKind(src.ScheduledAt.Value, DateTimeKind.Utc)
                : (DateTime?)null));
        }
    }
}
