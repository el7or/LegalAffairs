using AutoMapper;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class NotificationSystemMappingProfile : Profile
    {
        public NotificationSystemMappingProfile()
        {

            #region Domain Models to API Dto: 

            CreateMap<NotificationSystem, NotificationSystemListItemDto>()
                .ForMember(res => res.Id, opt => opt.MapFrom(c => c.NotificationId))
                .ForMember(res => res.Type, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.Type)))
                .ForMember(res => res.Text, opt => opt.MapFrom(u => u.Notification.Text))
                .ForMember(res => res.CreatedOn, opt => opt.MapFrom(c => c.Notification.CreatedOn))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.Notification.CreatedOn)))
                .ForMember(res => res.CreationTime, opt => opt.MapFrom(c => DateTimeHelper.GetFullTime(c.Notification.CreatedOn)));

            CreateMap<NotificationSystem, NotificationSystemDetailsDto>()
                .ForMember(res => res.Id, opt => opt.MapFrom(c => c.NotificationId))
                .ForMember(res => res.Type, opt => opt.MapFrom(c => EnumExtensions.GetDescription(c.Type)))
                .ForMember(res => res.Text, opt => opt.MapFrom(u => u.Notification.Text));

            #endregion

        }
    }
}
