using AutoMapper;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class NotificationSMSMappingProfile : Profile
    {
        public NotificationSMSMappingProfile()
        {

            #region Domain Models to API Dto: 


            CreateMap<NotificationSMS, NotificationSMSListItemDto>()
                .ForMember(res => res.PhoneNumber, opt => opt.MapFrom(u => u.User.PhoneNumber))
                .ForMember(res => res.Text, opt => opt.MapFrom(u => u.Notification.Text));

            CreateMap<NotificationSMS, NotificationSMSDetailsDto>()
                .ForMember(res => res.Text, opt => opt.MapFrom(u => u.Notification.Text));

            #endregion



        }
    }
}
