using Hangfire;
using Hangfire.Annotations;
using Hangfire.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class RecurringJobsService : BackgroundService
    {
        private readonly IRecurringJobManager _recurringJobs;
        private readonly ILogger<RecurringJobScheduler> _logger;
        private readonly HangfireConfigModel _hangfireConfig;

        public RecurringJobsService(
            [NotNull] IRecurringJobManager recurringJobs,
            [NotNull] ILogger<RecurringJobScheduler> logger,
            [NotNull] HangfireConfigModel hangfireConfig)
        {
            _recurringJobs = recurringJobs ?? throw new ArgumentNullException(nameof(recurringJobs));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hangfireConfig = hangfireConfig ?? throw new ArgumentNullException(nameof(hangfireConfig));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Creating the recurring jobs.");

                _recurringJobs.AddOrUpdate<INotificationSMSService>("SendSMSs", m => m.SendSMSs(), _hangfireConfig.HourlyIntervalCron);
                _recurringJobs.AddOrUpdate<INotificationEmailService>("SendEmails", m => m.SendEmails(), _hangfireConfig.HourlyIntervalCron);
                _recurringJobs.AddOrUpdate<ICaseService>("DetermineJudgment", m => m.DetermineJudgment(), _hangfireConfig.HourlyIntervalCron);
                _recurringJobs.AddOrUpdate<IAttachmentService>("AttachmentsCleanup", m => m.Cleanup(), _hangfireConfig.HourlyIntervalCron);
                _recurringJobs.AddOrUpdate<IHearingService>("FinishHearing", m => m.FinishHearing(), _hangfireConfig.HourlyIntervalCron);
                _recurringJobs.AddOrUpdate<IHearingService>("CloseHearing", m => m.CloseHearing(), _hangfireConfig.HourlyIntervalCron);
                _recurringJobs.AddOrUpdate<IObjectionPermitRequestService>("ExpiredObjections", m => m.EndExpiredObjections(), _hangfireConfig.DailyIntervalCorn);
                _recurringJobs.AddOrUpdate<IChangeResearcherToHearingRequestService>("CanceledChangeResearcherToHearingRequests", m => m.CanceledChangeResearcherToHearingRequests(), _hangfireConfig.HourlyIntervalCron);
                _recurringJobs.AddOrUpdate<ICaseService>("NotificationsForNotRecordedObjection", m => m.NotificationNotRecordedObjection(), _hangfireConfig.DailyIntervalCorn);
                _recurringJobs.AddOrUpdate<IHearingService>("NotifyManagerWithInCompletedHearingOutOfDate", m => m.NotifyManagerWithInCompletedHearingOutOfDate(), _hangfireConfig.DailyIntervalCorn);
                _recurringJobs.AddOrUpdate<IHearingService>("NotifyResearcherToCompleteHearingOutOfDate", m => m.NotifyResearcherToCompleteHearingOutOfDate(), _hangfireConfig.DailyIntervalCorn);
                _recurringJobs.AddOrUpdate<IHearingService>("NotifyUsersWithApproachHearingDate", m => m.NotifyUsersWithApproachHearingDate(), _hangfireConfig.DailyIntervalCorn);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while creating recurring jobs.", ex);

                return Task.FromException(ex);
            }
        }
    }
}
