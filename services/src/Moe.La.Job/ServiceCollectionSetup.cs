using Microsoft.Extensions.DependencyInjection;
using Moe.La.Common.Resources;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.Repositories;
using Moe.La.Integration;
using Moe.La.Integration.Interfaces;
using Moe.La.ServiceInterface;

namespace Moe.La.Job
{
    public static class ServiceCollectionSetup
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMoeEmailIntegrationService, MoeEmailIntegrationService>();
            services.AddScoped<IMoeSmsIntegrationService, MoeSmsIntegrationService>();
            services.AddScoped<INotificationEmailService, NotificationEmailService>();
            services.AddScoped<INotificationSMSService, NotificationSMSService>();
            services.AddScoped<ICaseService, CaseService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IHearingService, HearingService>();
            services.AddScoped<IHangfireAuthService, HangfireAuthService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IResearchsConsultantService, ResearchsConsultantService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICaseResearcherService, CaseResearcherService>();
            services.AddScoped<IHearingUpdateService, HearingUpdateService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IObjectionPermitRequestService, ObjectionPermitRequestService>();
            services.AddScoped<IChangeResearcherToHearingRequestService, ChangeResearcherToHearingRequestService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<INotificationEmailRepository, NotificationEmailRepository>();
            services.AddScoped<INotificationSMSRepository, NotificationSMSRepository>();
            services.AddScoped<ICaseRepository, CaseRepository>();
            services.AddScoped<ICaseTransactionRepository, CaseTransactionRepository>();
            services.AddScoped<ICaseResearchersRepository, CaseResearchersRepository>();
            services.AddScoped<IResearcherConsultantRepository, ResearcherConsultantRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IHearingRepository, HearingRepository>();
            services.AddScoped<IHearingUpdateRepository, HearingUpdateRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IHangfireAuthRepository, HangfireAuthRepository>();
            services.AddScoped<IRequestTransactionRepository, RequestTransactionRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IRequestTransactionRepository, RequestTransactionRepository>();
            services.AddScoped<IObjectionPermitRequestRepository, ObjectionPermitRequestRepository>();

            services.AddScoped<IChangeResearcherToHearingRequestRepository, ChangeResearcherToHearingRequestRepository>();
            services.AddScoped<IBoardMeetingRepository, BoardMeetingRepository>();
            services.AddScoped<IAddingObjectionLegalMemoToCaseRequestRepository, AddingObjectionLegalMemoToCaseRequestRepository>();
        }

        public static void AddLocalizationResources(this IServiceCollection services)
        {
            services.AddSingleton<Localization<Messages>>();
        }
    }
}
