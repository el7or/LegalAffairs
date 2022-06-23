using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Dtos.Consultations;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class ConsultationTransactionMappingProfile : Profile
    {
        public ConsultationTransactionMappingProfile()
        {
            #region API Dto to Domain:  

            CreateMap<ConsultationTransactionDto, ConsultationTransaction>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region Domain Models to API Dto: 

            CreateMap<ConsultationTransaction, ConsultationTransactionListDto>()
                .ForMember(m => m.ConsultationStatus, opt => opt.MapFrom(r => new KeyValuePairsDto<int> { Id = (int)r.Consultation.Status, Name = EnumExtensions.GetDescription(r.Consultation.Status) }))
                .ForMember(m => m.TransactionType, opt => opt.MapFrom(r => new KeyValuePairsDto<int> { Id = (int)r.TransactionType, Name = EnumExtensions.GetDescription(r.TransactionType) }))
                .ForMember(res => res.CreatedByUser, opt => opt.MapFrom(c => c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.CreatedOnTime, opt => opt.MapFrom(c => DateTimeHelper.GetFullTime(c.CreatedOn)));
            //.ForMember(res => res.CreatedByRole, opt => opt.MapFrom(c => c.CreatedByUser.UserRoles.FirstOrDefault().Role.NameAr));

            #endregion


        }
    }
}
