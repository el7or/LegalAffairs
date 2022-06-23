using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            #region API Dto to Domain:  

            CreateMap<RequestTransactionDto, RequestTransaction>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AddingLegalMemoToHearingRequestDto, RequestTransaction>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CaseSupportingDocumentRequestDto, RequestTransaction>()
               .ForMember(m => m.RequestId, opt => opt.MapFrom(r => r.Id))
               .ForMember(m => m.RequestStatus, opt => opt.MapFrom(r => r.Request.RequestStatus))
               .ForMember(m => m.Id, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<AttachedLetterRequestDto, RequestTransaction>()
               .ForMember(m => m.RequestId, opt => opt.MapFrom(r => r.ParentId))
               .ForMember(m => m.RequestStatus, opt => opt.MapFrom(r => r.Request.RequestStatus))
               .ForMember(m => m.Id, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<AddingLegalMemoToHearingRequestDto, RequestTransaction>()
               .ForMember(m => m.RequestId, opt => opt.MapFrom(r => r.Id))
               .ForMember(m => m.RequestStatus, opt => opt.MapFrom(r => r.Request.RequestStatus))
               .ForMember(m => m.Id, opt => opt.Ignore())
               .ReverseMap();

            #endregion

            #region Domain Models to API Dto: 

            CreateMap<RequestTransaction, RequestTransactionListDto>()
                .ForMember(m => m.RequestStatus, opt => opt.MapFrom(r => new KeyValuePairsDto<int> { Id = (int)r.RequestStatus, Name = EnumExtensions.GetDescription(r.RequestStatus) }))
                .ForMember(m => m.TransactionType, opt => opt.MapFrom(r => new KeyValuePairsDto<int> { Id = (int)r.TransactionType, Name = EnumExtensions.GetDescription(r.TransactionType) }))
                .ForMember(res => res.CreatedByUser, opt => opt.MapFrom(c => c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.CreatedOnTime, opt => opt.MapFrom(c => DateTimeHelper.GetFullTime(c.CreatedOn)))
                .ForMember(res => res.CreatedById, opt => opt.MapFrom(c => c.CreatedBy.ToString()));
            //.ForMember(res => res.CreatedByRole, opt => opt.MapFrom(c => c.CreatedByUser.UserRoles.FirstOrDefault().Role.NameAr));

            #endregion


        }
    }
}
