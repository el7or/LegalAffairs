using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class InvestigationMappingProfile : Profile
    {
        public InvestigationMappingProfile()
        {
            CreateMap<Investigation, InvestigationListItemDto>()
                .ForMember(res => res.StartDateHigri, opt => opt.MapFrom(i => DateTimeHelper.GetHigriDate(i.StartDate)))
                .ForMember(res => res.StartTime, opt => opt.MapFrom(c => DateTimeHelper.GetTime(c.StartDate)))
                .ForMember(res => res.InvestigatorFullName, opt => opt.MapFrom(i => i.Investigator.FirstName + " " + i.Investigator.LastName))
                .ForMember(res => res.InvestigationStatus, opt => opt.MapFrom(i => EnumExtensions.GetDescription(i.InvestigationStatus)));

            CreateMap<Investigation, InvestigationDetailsDto>();

            CreateMap<Investigation, InvestigationDto>();

            CreateMap<InvestigationDto, Investigation>()
                .ForMember(res => res.Id, opt => opt.Ignore());

        }
    }
}
