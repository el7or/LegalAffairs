using AutoMapper;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;

namespace Moe.La.Core.Mapping
{
    public class NotificationEmailMappingProfile : Profile
    {
        public NotificationEmailMappingProfile()
        {

            #region Domain Models to API Dto: 


            CreateMap<NotificationEmail, NotificationEmailListItemDto>()
                .ForMember(res => res.Email, opt => opt.MapFrom(u => u.User.Email))
                .ForMember(res => res.Text, opt => opt.MapFrom(u => u.Notification.Text));

            CreateMap<NotificationEmail, NotificationEmailDetailsDto>()
                .ForMember(res => res.Text, opt => opt.MapFrom(u => u.Notification.Text));

            #endregion



        }
    }
}
