using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moe.La.MockIntegrationService
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
            //client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            //client.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var randomResource = new Random().Next(1, 3);

                switch (randomResource)
                {
                    case 1:
                        await PushMoeenCase();
                        break;
                    case 2:
                        await PushNajizCase();
                        break;
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(15 * 1000, stoppingToken);
            }
        }

        private async Task PushMoeenCase()
        {
            var id = new Random().Next(1, 100);

            var _case = new MoeenCaseDto()
            {
                MoeenRef = "رقم المعاملة في معين " + id,
                MoeenId = "رقم الدعوى في معين " + id,
                Subject = "عنوان قضية من نظام معين رقمها " + id,
                LegalStatus = new Random().Next(1, 3) == 1 ? "مدعية" : "مدعى عليها",
                Court = "المحكمة  " + new Random().Next(1, 35),
                CircleNumber = new Random().Next(1, 20).ToString(),
                LitigationType = "درجة الترافع" + new Random().Next(1, 3),
                StartDate = DateTime.Now.AddDays(-1),
                ReceivedDate = DateTime.Now,
                ReceivedStatus = ReceivedStatuses.Successful,
                IsDuplicated = false,
                CaseDescription = "دعوى لصالح الوزارة ضد شركة المقاولات",
                RelatedCaseId = new Random().Next(1, 10),
                Parties =
                {
                    new MoeenPartyDto()
                    {
                        Name = "party " + id,
                        AddressDetails = "address " + id,
                        IdentityValue = "Identity " + id,
                        AttorneyPartyName = "AttorneyPartyName " + id
                    }
                },
                Hearings =
                {
                    new MoeenHearingDto()
                    {
                         HearingNumber=1,
                         CircleNumber=new Random().Next(1, 20).ToString(),
                         HearingDate=DateTime.Now,
                         HearingTime=DateTime.Now.ToString("hh:mm"),
                         Court = "المحكمة  "+ new Random().Next(1, 35),
                         HearingDesc="HearingDesc"+ id,
                         Status = HearingStatuses.Scheduled,
                         Type = HearingTypes.Pleading
                    }
                }
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.MoeenServiceUri);
                //client.BaseAddress = new Uri("https://localhost:44387/api/integration/moeen-case-log");

                var Content = new StringContent(JsonConvert.SerializeObject(_case), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(client.BaseAddress, Content);

                if (response.IsSuccessStatusCode)
                {
                    var sentCase = await response.Content.ReadAsAsync<MoeenCaseDto>();
                    _logger.LogInformation("Moeen case sent:");
                    _logger.LogInformation(JsonConvert.SerializeObject(sentCase));
                }

            }
        }

        private async Task PushNajizCase()
        {
            var id = new Random().Next(1, 100);

            var _case = new NajizCaseDto()
            {
                NajizRef = "رقم المعاملة في ناجز " + id,
                NajizId = "رقم الطلب في ناجز " + id,
                NajizCaseNo = "رقم القضية في ناجز " + id,
                Subject = "عنوان قضية من نظام معين رقمها " + id,
                LegalStatus = new Random().Next(1, 3) == 1 ? "مدعية" : "مدعى عليها",
                Court = "المحكمة  " + new Random().Next(1, 35),
                CircleNumber = new Random().Next(1, 20).ToString(),
                LitigationType = "درجة الترافع" + new Random().Next(1, 3),
                StartDate = DateTime.Now.AddDays(-1),
                ReceivedDate = DateTime.Now,
                ReceivedStatus = ReceivedStatuses.Successful,
                IsDuplicated = false,
                CaseDescription = "دعوى لصالح الوزارة ضد شركة المقاولات",
                RelatedCaseId = new Random().Next(1, 10),
                Parties =
                {
                    new NajizPartyDto()
                    {
                        Name = "party " + id,
                        AddressDetails = "address " + id,
                        IdentityValue = "Identity " + id,
                        AttorneyPartyName = "AttorneyPartyName " + id
                    }
                },
                Hearings =
                {
                    new NajizHearingDto()
                    {
                         HearingNumber=1,
                         CircleNumber=new Random().Next(1, 20).ToString(),
                         HearingDate=DateTime.Now,
                         HearingTime=DateTime.Now.ToString("hh:mm"),
                         Court = "المحكمة  "+ new Random().Next(1, 35),
                         HearingDesc="HearingDesc"+ id,
                         Status = HearingStatuses.Scheduled,
                         Type = HearingTypes.Pleading
                    }
                }
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.NajizServiceUri);
                //client.BaseAddress = new Uri("https://localhost:44387/api/integration/najiz-case-log");

                var Content = new StringContent(JsonConvert.SerializeObject(_case), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(client.BaseAddress, Content);

                if (response.IsSuccessStatusCode)
                {
                    var sentCase = await response.Content.ReadAsAsync<NajizCaseDto>();
                    _logger.LogInformation("Najiz case sent:");
                    _logger.LogInformation(JsonConvert.SerializeObject(sentCase));
                }

            }
        }

        //List<NajizCaseDto> NajizCases = new List<NajizCaseDto>()
        //{
        //    new NajizCaseDto(){ NajizRef ="Ref 1",NajizCaseNo="No 1",NajizId="Id 1",Subject="subject 1",LegalStatus="1",Court="Court 1"},
        //    new NajizCaseDto(){ NajizRef ="Ref 2",NajizCaseNo="No 2",NajizId="Id 2",Subject="subject 2",LegalStatus="1",Court="Court 2"},
        //    new NajizCaseDto(){ NajizRef ="Ref 3",NajizCaseNo="No 3",NajizId="Id 3",Subject="subject 3",LegalStatus="1",Court="Court 3"}
        //};

    }


    public class WorkerOptions
    {
        public string MoeenServiceUri { get; set; }
        public string NajizServiceUri { get; set; }
    }
}
