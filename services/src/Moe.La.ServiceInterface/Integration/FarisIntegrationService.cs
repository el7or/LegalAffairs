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
    public class FarisIntegrationService : IFarisIntegrationService
    {
        private readonly IInvestiationRecordPartyService _investiationRecordPartyService;
        private readonly ILogger<FarisIntegrationService> _logger;

        public FarisIntegrationService(IInvestiationRecordPartyService investiationRecordPartyService, ILogger<FarisIntegrationService> logger)
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

        private static List<FaresUserDto> CreateUsers()
        {
            return new List<FaresUserDto>
            {
                new FaresUserDto
                {
                    Id = 1,
                    Name = "احمد محمد محمود محمد",
                    AssignedWork = "assigned work to ahmed",
                    IdentityNumber = "1111111111",
                    WorkLocation="location work for ahmed"
                },
                new FaresUserDto
                {
                    Id = 2,
                    Name = "بسمة ابراهيم",
                    AssignedWork = "assigned work to basma",
                    IdentityNumber = "2222222222",
                    WorkLocation="location work for basme"
                },
                new FaresUserDto
                {
                    Id = 3,
                    Name = "اسلام محمد احمد",
                    AssignedWork = "assigned work to eslam",
                    IdentityNumber = "3333333333",
                    WorkLocation="location work for ahmed"
                },
                new FaresUserDto
                {
                    Id = 4,
                    Name = "على محمد ابراهيم",
                    AssignedWork = "assigned work to ali",
                    IdentityNumber = "4444444444",
                    WorkLocation="location work for basme"
                },
            };
        }

        private static List<InvestigationRecordPartyDetailsDto> CreateParties()
        {
            return new List<InvestigationRecordPartyDetailsDto>
            {
                new InvestigationRecordPartyDetailsDto
                {
                    Id = 1,
                    PartyName = "احمد محمد محمود محمد",
                    BirthDate = DateTime.Now,
                    BirthDateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now),
                    AppointmentStatus =new  KeyValuePairsDto<int>{Id=(int) AppointmentStatus.OnTheJob,Name=EnumExtensions.GetDescription(AppointmentStatus.OnTheJob) },
                    LastQualificationAttained = "admin@moe.sa",
                    AssignedWork = "assigned work",
                    IdentityNumber = "1234567891",
                    WorkLocation="location  work",
                    CommencementDate=DateTime.Now,
                    CommencementDateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now),
                    IdentityDate=DateTime.Now,
                    IdentityDateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now),
                    IdentitySource="",
                    //InvestigationRecordPartyType=new KeyValuePairsDto<int>(){ Id=1,Name="dd"},
                    StaffType=new  KeyValuePairsDto<int>{Id=(int)StaffType.Educational,Name=EnumExtensions.GetDescription(StaffType.Educational) },
                    InvestigationPartyPenalties=
                    {
                        new InvestigationRecordPartyPenaltyDto
                        {
                            Penalty="عقاب1",
                            Reasons="سبب1",
                            DecisionNumber=11,
                            Date=DateTime.Now
                            ,DateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now)
                        },
                        new InvestigationRecordPartyPenaltyDto
                        {
                            Penalty="عقاب2",
                            Reasons="سبب2",
                            DecisionNumber=22,
                            Date=DateTime.Now
                           ,DateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now)
                        }
                    },
                    Evaluations =
                    {
                        new EvaluationDto
                        {
                            Percentage=70,
                            Year=2000
                        },
                        new EvaluationDto
                        {
                            Percentage=70,
                            Year=2001
                        },
                        new EvaluationDto
                        {
                            Percentage=70,
                            Year=2002
                        }
                    }
                },
                new InvestigationRecordPartyDetailsDto
                {
                    Id = 2,
                    PartyName = "هاشم محمد محمود محمد",
                    BirthDate = DateTime.Now,
                    BirthDateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now),
                    AppointmentStatus =new  KeyValuePairsDto<int>{Id=(int) AppointmentStatus.OnTheJob,Name=EnumExtensions.GetDescription(AppointmentStatus.OnTheJob) },
                    LastQualificationAttained = "admin@moe.sa",
                    AssignedWork = "assigned  work  ",
                    IdentityNumber = "1234567892",
                    WorkLocation="location  work",
                    CommencementDate=DateTime.Now,
                    CommencementDateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now),
                    IdentityDate=DateTime.Now,
                    IdentityDateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now),
                    IdentitySource="",
                    //InvestigationRecordPartyTypeId=1,
                    StaffType=new  KeyValuePairsDto<int>{Id=(int)StaffType.Educational,Name=EnumExtensions.GetDescription(StaffType.Educational) },
                    InvestigationPartyPenalties=
                    {
                        new InvestigationRecordPartyPenaltyDto
                        {
                            Penalty="عقاب1",
                            Reasons="سبب1",
                            DecisionNumber=11,
                            Date=DateTime.Now
                            ,DateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now)
                        },
                        new InvestigationRecordPartyPenaltyDto
                        {
                            Penalty="عقاب2",
                            Reasons="سبب2",
                            DecisionNumber=22,
                            Date=DateTime.Now,
                            DateOnHijri=DateTimeHelper.GetHigriDate(DateTime.Now)
                        }
                    },
                    Evaluations =
                    {
                        new EvaluationDto
                        {
                            Percentage=70,
                            Year=2000
                        },
                        new EvaluationDto
                        {
                            Percentage=70,
                            Year=2001
                        },
                        new EvaluationDto
                        {
                            Percentage=70,
                            Year=2002
                        }
                    }
                }
            };
        }

        public ReturnResult<FaresUserDto> GetUserAsync(string id)
        {
            try
            {
                var errors = new List<string>();

                var users = CreateUsers();

                var result = users.Find(u => u.IdentityNumber == id);



                if (errors.Any())
                {
                    return new ReturnResult<FaresUserDto>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status400BadRequest,
                        ErrorList = errors,
                        Data = result
                    };
                }

                return new ReturnResult<FaresUserDto>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<FaresUserDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }
    }
}
