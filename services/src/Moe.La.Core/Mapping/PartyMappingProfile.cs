using AutoMapper;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class PartyMappingProfile : Profile
    {
        public PartyMappingProfile()
        {

            #region Domain Models to API Dto: 

            CreateMap<Party, PartyListItemDto>()
                .ForMember(res => res.PartyTypeName, opt => opt.MapFrom(a => EnumExtensions.GetDescription(a.PartyType)))
                .ForMember(res => res.IdentityType, opt => opt.MapFrom(a => a.IdentityType.Name))
                .ForMember(res => res.City, opt => opt.MapFrom(a => a.City.Name));

            CreateMap<Party, PartyDetailsDto>()
                .ForMember(res => res.PartyTypeName, opt => opt.MapFrom(a => EnumExtensions.GetDescription(a.PartyType)))
                .ForMember(res => res.IdentityType, opt => opt.MapFrom(a => a.IdentityType.Name))
                .ForMember(res => res.City, opt => opt.MapFrom(a => a.City.Name));

            CreateMap<Party, PartyDto>()
                .ForMember(res => res.PartyTypeName, opt => opt.MapFrom(a => EnumExtensions.GetDescription(a.PartyType)))
                .ForMember(res => res.IdentityType, opt => opt.MapFrom(a => a.IdentityType.Name))
                .ReverseMap();

            CreateMap<CaseParty, CasePartyDto>()
              .ForMember(res => res.CaseId, opt => opt.MapFrom(a => a.CaseId))
              .ForMember(res => res.PartyId, opt => opt.MapFrom(a => a.PartyId))
              .ForMember(res => res.PartyStatusName, opt => opt.MapFrom(a => a.PartyStatus != null ? EnumExtensions.GetDescription(a.PartyStatus) : ""))
              .ReverseMap();

            #endregion

            #region API Dto to Domain:  
            CreateMap<PartyDto, Party>()
                .ForMember(res => res.IdentityType, opt => opt.Ignore());
            #endregion

        }
    }
}
