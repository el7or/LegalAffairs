using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moe.La.Common;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moe.La.MockMoamalaService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WorkerOptions _options;

        public Worker(ILogger<Worker> logger, WorkerOptions options)
        {
            _logger = logger;
            _options = options;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await PushMoamla();

                await Task.Delay(15 * 1000, stoppingToken);
            }
        }
        private async Task PushMoamla()
        {
            var id = new Random().Next(1, 10000);
            var moamlaSubjects = new string[3] { " ", "انشاء قضية", "انشاء تحقيق" };
            var subjectId = new Random().Next(1, 2);
            var centerIdTo = new Random().Next(1, 2);
            var centerIdFrom = new Random().Next(1, 2);
            var centers = new string[4] { "", "مركز 1", "مركز 2", "مركز 3" };
            var groups = new string[4] { "", "إدارة 1", "إدارة 2", "إدارة 3" };
            var itemTypeId = new Random().Next(1, 3);
            var itemTypes = new string[4] { "", "نوع معاملة 1", "نوع معاملة 2", "نوع معاملة 3" };
            var groupIdTo = new Random().Next(1, 3);
            var groupIdFrom = new Random().Next(1, 3);
            var itemPrivacy = new Random().Next(1, 3);
            var moamala = new MoamalaRaselDto()
            {
                ItemNumber = new Random().Next(1, 100),
                UnifiedNumber = "الرقم الموحد " + id,
                CustomNumber = "رقم القيد " + id,
                PreviousItemNumber = id,
                Subject = "معاملة بشأن " + moamlaSubjects[subjectId],
                Comments = "ملاحظة" + " " + id,
                HijriCreatedDate = DateTimeHelper.GetHigriDateInt(DateTime.Now),
                GregorianCreatedDate = DateTime.Now,
                CenterIdTo = null,
                CenterArabicNameTo = null,
                CenterIdFrom = centerIdFrom,
                CenterArabicNameFrom = centers[centerIdFrom],
                GroupIdTo = groupIdTo,
                GroupNameTo = groups[centerIdTo],
                GroupIdFrom = null,
                GroupNameFrom = null,
                ItemPrivacy = (ConfidentialDegrees)itemPrivacy,
                ItemPrivacyName = "",
                ItemType = null,
                ItemTypeName = null
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.MoamalatServiceUri);
                //client.BaseAddress = new Uri("https://localhost:44387/api/integration/moamala-log");

                var Content = new StringContent(JsonConvert.SerializeObject(moamala), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(client.BaseAddress, Content);

                if (response.IsSuccessStatusCode)
                {
                    var sentTransaction = await response.Content.ReadAsAsync<MoamalaRaselDto>();
                    _logger.LogInformation("Moamala sent:");
                    _logger.LogInformation(JsonConvert.SerializeObject(sentTransaction));
                }
            }
        }
    }

    public class WorkerOptions
    {
        public string MoamalatServiceUri { get; set; }
    }
}
