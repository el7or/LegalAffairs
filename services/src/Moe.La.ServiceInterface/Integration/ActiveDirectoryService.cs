//using Microsoft.Extensions.Logging;
//using Moe.La.Core.Dtos;
//using Moe.La.Core.Enums;
//using Moe.La.Core.Interfaces.Repositories;
//using Moe.La.Core.Interfaces.Services;
//using Moe.La.Core.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Moe.La.ServiceInterface
//{
//    public class ActiveDirectoryService : IActiveDirectoryService
//    {
//        private readonly IUserRepository _userRepository;
//        private readonly ILogger<ActiveDirectoryService> _logger;

//        public ActiveDirectoryService(IUserRepository userRepository, ILogger<ActiveDirectoryService> logger)
//        {
//            _userRepository = userRepository;
//            _logger = logger;
//        }

//        public ReturnResult<List<UserDto>> GetAllAsync()
//        {
//            try
//            {
//                var entities = CreateUsers();

//                return new ReturnResult<List<UserDto>>
//                {
//                    IsSuccess = true,
//                    StatusCode = HttpStatuses.Status200OK,
//                    Data = entities
//                };
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message);

//                return new ReturnResult<List<UserDto>>
//                {
//                    IsSuccess = false,
//                    StatusCode = HttpStatuses.Status500InternalServerError,
//                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
//                };
//            }
//        }

//        public async Task<ReturnResult<UserDto>> GetAsync(string searchText)
//        {
//            try
//            {
//                var errors = new List<string>();

//                var users = CreateUsers();

//                var result = users.Find(u => u.IdentityNumber == searchText
//                || u.UserName == searchText
//                || (u.FirstName + " " + u.LastName == searchText));

//                if (result == null)
//                {
//                    errors.Add("لا يوجد مستخدم لهذا البحث");
//                }
//                else
//                {
//                    if (await _userRepository.CheckUserExists(result.UserName))
//                    {
//                        errors.Add("هذا المستخدم موجود مسبقاً");
//                    }
//                }

//                if (errors.Any())
//                {
//                    return new ReturnResult<UserDto>
//                    {
//                        IsSuccess = false,
//                        StatusCode = HttpStatuses.Status400BadRequest,
//                        ErrorList = errors,
//                        Data = result
//                    };
//                }

//                return new ReturnResult<UserDto>
//                {
//                    IsSuccess = true,
//                    StatusCode = HttpStatuses.Status200OK,
//                    Data = result
//                };
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message, searchText);

//                return new ReturnResult<UserDto>
//                {
//                    IsSuccess = false,
//                    StatusCode = HttpStatuses.Status500InternalServerError,
//                    ErrorList = new List<string>() { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
//                };
//            }
//        }

//        private static List<UserDto> CreateUsers()
//        {
//            return new List<UserDto>
//            {
//                new UserDto
//                {
//                    Id = new Guid("f4dd3f12-d6ec-4e9c-a2b2-ed7cfae15c7b"),
//                    FirstName = "احمد",
//                    LastName = "الرويشد",
//                    UserName = "1111111111",
//                    Email = "admin@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "1234567891",
//                    EmployeeNo="1234567891",
//                },
//                new UserDto
//                {
//                    Id = new Guid("c1cc4289-0e4a-4e6b-9b21-6b6c39841eb1"),
//                    FirstName = "عبيد",
//                    LastName = "الدوسري",
//                    UserName = "1111111112",
//                    Email = "GeneralSupervisor@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "1234567892",
//                    EmployeeNo="1234567892",
//                },
//                new UserDto
//                {
//                    Id = new Guid("b4dfa7a8-94b6-46d3-9a8f-ca4ba323d6b3"),
//                    FirstName = "ناصر",
//                    LastName = "العتيبي",
//                    UserName = "1111111113",
//                    Email = "LitigationManager@moe.sa",
//                    GeneralManagementId = 1,
//                   IdentityNumber = "1234567893",
//                   EmployeeNo="1234567893",
//                },
//                new UserDto
//                {
//                    Id = new Guid("3a350fdf-ea98-4bdf-b230-1b8f378ba700"),
//                    FirstName = "صالح",
//                    LastName = "المطيري",
//                    UserName = "1111111114",
//                    Email = "RegionsSupervisor@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "1234567894",
//                    EmployeeNo="1234567894",
//                },
//                new UserDto
//                {
//                    Id = new Guid("773751be-8998-46f4-8693-ea1b5d877b8f"),
//                    FirstName = "خالد",
//                    LastName = "العنزي",
//                    UserName = "1111111115",
//                    Email = "BranchManager@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "1234567895",
//                    EmployeeNo="1234567895",
//                },
//                new UserDto
//                {
//                    Id = new Guid("22a1955d-7ea2-4dbc-b8fc-b47710188635"),
//                    FirstName = "زياد",
//                    LastName = "المالكي",
//                    UserName = "1111111116",
//                    Email = "MainBoardHead@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "1234567896",
//                    EmployeeNo="1234567896",
//                },
//                new UserDto
//                {
//                    Id = new Guid("18b96059-0101-42d8-8f31-cf52c9a9e8f6"),
//                    FirstName = "احمد",
//                    LastName = "الحميد",
//                    UserName = "1111111117",
//                    Email = "BoardHead@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "1234567897",
//                    EmployeeNo="1234567897",
//                },
//                new UserDto
//                {
//                    Id = new Guid("51251117-896b-4fe9-8da2-545c21195f63"),
//                    FirstName = "عبدالرحمن",
//                    LastName = "الحمود",
//                    UserName = "1111111118",
//                    Email = "BoardMember@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "1234567898",
//                    EmployeeNo="1234567898",
//                },
//                new UserDto
//                {
//                    Id = new Guid("c0f56471-7b26-4b0d-986e-1e0cd8ce512f"),
//                    FirstName = "ذياب",
//                    LastName = "الشمري",
//                    UserName = "1111111119",
//                    Email = "LegalConsultant1@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "1234567899",
//                    EmployeeNo="1234567899",
//                },
//                new UserDto
//                {
//                    Id = new Guid("b7180aaa-891d-48f4-b706-7a399610dff4"),
//                    FirstName = "علي",
//                    LastName = "الهلالي",
//                    UserName = "1111111120",
//                    Email = "LegalConsultant2@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "2134567890",
//                    EmployeeNo="2234567890",
//                },
//                new UserDto
//                {
//                    Id = new Guid("aa78b684-9a4f-482c-b8c9-f3fc200690a5"),
//                    FirstName = "حمد",
//                    LastName = "عبدالله",
//                    UserName = "1111111121",
//                    Email = "LegalResearcher1@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "2134567891",
//                    EmployeeNo="2234567891",
//                },
//                new UserDto
//                {
//                    Id = new Guid("477829d0-2471-4bae-8c03-4ed1e4f1ca1b"),
//                    FirstName = "طلال",
//                    LastName = "الحمراني",
//                    UserName = "1111111122",
//                    Email = "LegalResearcher2@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "2134567892",
//                    EmployeeNo="2234567892",
//                },
//                new UserDto
//                {
//                    Id = new Guid("eef15323-953e-4d8e-a287-3e5ffb2edb80"),
//                    FirstName = "عبدالله",
//                    LastName = "الشهراني",
//                    UserName = "1111111123",
//                    Email = "Distributor@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "2134567893",
//                    EmployeeNo="2234567893",
//                },
//                new UserDto
//                {
//                    Id = new Guid("488829d0-2471-4bae-8c03-4ed1e4f1ca1b"),
//                    FirstName = "عبدالله",
//                    LastName = "القصيبي",
//                    UserName = "1111111125",
//                    Email = "test@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "2134567895",
//                    EmployeeNo="2234567894",
//                },
//                new UserDto
//                {
//                    Id = new Guid("488829d1-2472-4bae-8c03-4ed1e4f1ca1b"),
//                    FirstName = "ابراهيم",
//                    LastName = "جبر",
//                    UserName = "1111111126",
//                    Email = "test2@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "2134567896",
//                    EmployeeNo="2234567895",
//                },
//                new UserDto
//                {
//                    Id = new Guid("488829d2-2472-4bae-8c03-4ed1e4f1ca1b"),
//                    FirstName = "تميم",
//                    LastName = "المعتوق",
//                    UserName = "1111111127",
//                    Email = "test3@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "2134567897",
//                    EmployeeNo="2234567896",
//                },
//                new UserDto
//                {
//                    Id = new Guid("488829d3-2472-4bae-8c03-4ed1e4f1ca1b"),
//                    FirstName = "سيف",
//                    LastName = "الحمود",
//                    UserName = "1111111128",
//                    Email = "test4@moe.sa",
//                    GeneralManagementId = 2,
//                    IdentityNumber = "2134567898",
//                    EmployeeNo="2234567897",
//                },
//                new UserDto
//                {
//                    Id = new Guid("488829d4-2472-4bae-8c03-4ed1e4f1ca1b"),
//                    FirstName = "محمد",
//                    LastName = "ابراهيم",
//                    UserName = "1111111129",
//                    Email = "test5@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "2134567899",
//                    EmployeeNo="2234567898",
//                },
//                new UserDto
//                {
//                    Id = new Guid("488829d5-2472-4bae-8c03-4ed1e4f1ca1b"),
//                    FirstName = "محمود",
//                    LastName = "الذيابي",
//                    UserName = "1111111130",
//                    Email = "test6@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "2034567890",
//                    EmployeeNo="2234567899",
//                },
//                new UserDto
//                {
//                    Id = new Guid("488829d5-2472-4bae-8c03-4ed1e4f1ca1b"),
//                    FirstName = "شيماء",
//                    LastName = "الذيابي",
//                    UserName = "1111111145",
//                    Email = "test7@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "2034567893",
//                    EmployeeNo="2034567893",
//                },
//                new UserDto
//                {
//                    Id = new Guid("488829d5-2472-4bae-8c03-4ed1e4f1ca1b"),
//                    FirstName = "سعيد",
//                    LastName = "الذيابي",
//                    UserName = "1111111146",
//                    Email = "test8@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "1111111146",
//                    EmployeeNo="2034567894",
//                },
//                new UserDto
//                {
//                    Id = new Guid("488829d5-2472-4bae-8c03-4ed1e4f1ca1b"),
//                    FirstName = "احمد",
//                    LastName = "الذيابي",
//                    UserName = "1111111147",
//                    Email = "test9@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "1111111147",
//                    EmployeeNo="2034567897",
//                },
//                new UserDto
//                {
//                    Id = new Guid("488829d5-2472-4bae-8c03-4ed1e4f1ca1b"),
//                    FirstName = "Unit Test User",
//                    LastName = "Unit Test User",
//                    UserName = "1111111199",
//                    Email = "unit-test@moe.sa",
//                    GeneralManagementId = 1,
//                    IdentityNumber = "1111111199",
//                    EmployeeNo="2034567899",
//                },
//            };
//        }

//    }
//}
