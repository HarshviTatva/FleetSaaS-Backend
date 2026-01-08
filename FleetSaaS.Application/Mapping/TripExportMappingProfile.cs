using AutoMapper;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Domain.Enum;
using FleetSaaS.Infrastructure.Common;

namespace FleetSaaS.Application.Mapping
{
    public class TripExportMappingProfile : Profile
    {
        public TripExportMappingProfile()
        {
            CreateMap<TripDTO, TripExportDTO>()
                 .ForMember(
                    d => d.VehicleDriverName,
                    opt => opt.MapFrom(
                        s => string.IsNullOrWhiteSpace(s.VehicleDriverName)
                            ? Fields.NOT_ASSIGNED
                            : s.VehicleDriverName
                    )
                    )
                 .ForMember(
                    d => d.DistanceCovered,
                    opt => opt.MapFrom(
                        s => s.DistanceCovered!=null
                            ? s.DistanceCovered
                            : 0
                       )
                    )
                 .ForMember(
                     d => d.ScheduledAt,
                     opt => opt.MapFrom(
                         s => s.ScheduledAt!=null
                             ? s.ScheduledAt
                             : Fields.NO_DATA
                     )
                 )
                .ForMember(d => d.Status,
                    opt => opt.MapFrom(s => Enum.GetName(typeof(TripStatus), s.Status)))
                .ForMember(d=>d.TripCode,opt => opt.MapFrom(s=>s.Name));
        }
    }
}
