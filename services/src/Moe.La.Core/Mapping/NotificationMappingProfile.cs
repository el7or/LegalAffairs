using AutoMapper;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {

            #region Domain Models to API Dto: 

            CreateMap<Notification, NotificationDto>()
                .ReverseMap();


            #endregion

            #region API Dto to Domain: 

            CreateMap<NotificationDto, Notification>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            //    .ForMember(c => c.Users, opt => opt.Ignore())
            //    .AfterMap((notification, notificationEntity) =>
            //    {
            //        foreach (var notificationUser in notification.Users)
            //            notificationEntity.Users.Add(new NotificationSystem
            //            {
            //                NotificationId = notification.Id,
            //                UserId = notificationUser.userId,
            //                IsRead=notificationUser.IsRead,
            //                IsEmailSent=notificationUser.IsEmailSent,
            //                IsSmsSent=notificationUser.IsSmsSent
            //            });                  
            //    });
            //CreateMap<NotificationSystemQueryObjectDto, NotificationSystemQueryObject>();

            #endregion

        }
    }
}
