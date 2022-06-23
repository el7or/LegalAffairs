using Microsoft.Extensions.Logging;
using Moe.La.Common;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class NoorIntegrationService : INoorIntegrationService
    {
        private readonly ILogger<NoorIntegrationService> _logger;
        private readonly IInvestiationRecordPartyService _investiationRecordPartyService;

        public NoorIntegrationService(IInvestiationRecordPartyService investiationRecordPartyService, ILogger<NoorIntegrationService> logger)
        {
            _investiationRecordPartyService = investiationRecordPartyService;
            _logger = logger;
        }

        public async Task<ReturnResult<InvestigationRecordPartyDetailsDto>> GetAsync(string searchText, int? investigationRecordId)
        {
            try
            {
                var errors = new List<string>();
                var parties = CreateParties();
                var result = parties.Find(u => u.IdentityNumber == searchText || u.PartyName == searchText);

                if (investigationRecordId != null)
                {
                    var checkPartyExistResult = await _investiationRecordPartyService.CheckPartyExistAsync(result.IdentityNumber, investigationRecordId);

                    if (checkPartyExistResult.Data)
                    {
                        errors.Add("هذا الطرف موجود مسبقاً لهذا المحضر ");
                    }
                }

                if (errors.Any())
                {
                    return new ReturnResult<InvestigationRecordPartyDetailsDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                return new ReturnResult<InvestigationRecordPartyDetailsDto>(true, HttpStatuses.Status200OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, searchText, investigationRecordId);

                return new ReturnResult<InvestigationRecordPartyDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        private static List<InvestigationRecordPartyDetailsDto> CreateParties()
        {
            return new List<InvestigationRecordPartyDetailsDto>
            {
                new InvestigationRecordPartyDetailsDto
                {
                    Id = 1,
                    PartyName = "هند محمد محمود محمد",
                    BirthDate = DateTime.Now,
                    BirthDateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now),
                    LastQualificationAttained = "الابتدائية",
                    AssignedWork = "",
                    IdentityNumber = "1234567893",
                    WorkLocation="",
                    CommencementDate=DateTime.Now,
                    CommencementDateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now),
                    IdentityDate=DateTime.Now,
                    IdentityDateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now),
                    IdentitySource="",
                    StaffType=new   KeyValuePairsDto<int>(){Id=(int)StaffType.Educational,Name=EnumExtensions.GetDescription(StaffType.Educational)},
                    AppointmentStatus=new   KeyValuePairsDto<int>(){Id=(int)AppointmentStatus.BlindHand,Name=EnumExtensions.GetDescription(AppointmentStatus.BlindHand)},
                    EducationalLevels=
                    {
                        new EducationalLevelDto
                        {
                            EducationLevel="ابتدائي",
                            Class="الاول",
                            ClassNumber="الاول",
                            ResidenceAddress="عنوان1"
                        },
                        new EducationalLevelDto
                        {
                            EducationLevel="متوسط",
                            Class="الثاني",
                            ClassNumber="3/2",
                            ResidenceAddress="عنوان2"
                        }
                    }
                },
                new InvestigationRecordPartyDetailsDto
                {
                    Id = 2,
                    PartyName = "هيثم محمد محمود محمد",
                    BirthDate = DateTime.Now,
                    AppointmentStatus=new   KeyValuePairsDto<int>(){Id=(int)AppointmentStatus.BlindHand,Name=EnumExtensions.GetDescription(AppointmentStatus.BlindHand)},
                    LastQualificationAttained = "admin@moe.sa",
                    AssignedWork = "",
                    IdentityNumber = "1234567892",
                    WorkLocation="1234567891",
                    CommencementDate=DateTime.Now,
                    IdentityDate=DateTime.Now,
                    IdentitySource="",
                    StaffType=new   KeyValuePairsDto<int>(){Id=(int)StaffType.Educational,Name=EnumExtensions.GetDescription(StaffType.Educational)},
                    EducationalLevels=
                    {
                        new EducationalLevelDto
                        {
                            EducationLevel="مستوى1",
                            Class="الاول",
                            ClassNumber="1/2",
                            ResidenceAddress="عنوان1"
                        },
                        new EducationalLevelDto
                        {
                            EducationLevel="مستوى2",
                            Class="الثاني",
                            ClassNumber="1/1",
                            ResidenceAddress="عنوان2"
                        }
                    }
                }
            };
        }
    }
}