using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.Infrastructure.DbContexts;
using Moe.La.Infrastructure.Repositories;
using Moe.La.Integration;
using Moe.La.Integration.Options;
using Moe.La.ServiceInterface;
using Moq;
using System;

namespace Moe.La.UnitTests
{
    public class ServiceHelper
    {
        public ServiceHelper(LaDbContext dbContext, IMapper mapper, IUserProvider userProvider,
            UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IDistributedCache cache)
        {
            Db = dbContext;
            Mapper = mapper;
            UserProvider = userProvider;
            UserManager = userManager;
            RoleManager = roleManager;
            Cache = cache;
        }

        public LaDbContext Db { get; private set; }

        public IMapper Mapper { get; private set; }

        public IUserProvider UserProvider { get; private set; }

        public UserManager<AppUser> UserManager { get; private set; }

        public RoleManager<AppRole> RoleManager { get; private set; }

        public IDistributedCache Cache { get; private set; }

        public PartyService CreatePartyService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var partyRepository = new PartyRepository(Db, Mapper, UserProvider);
            return new PartyService(partyRepository, new Mock<ILogger<PartyService>>().Object);
        }
        public MinistryDepartmentService CreateMinistryDepartmentService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var ministryDepartmentRepository = new MinistryDepartmentRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);
            return new MinistryDepartmentService(ministryDepartmentRepository, new Mock<ILogger<MinistryDepartmentService>>().Object);
        }

        public CourtService CreateCourtService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var courtRepository = new CourtRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);
            return new CourtService(courtRepository, new Mock<ILogger<CourtService>>().Object);
        }

        public LegalMemoService CreateLegalMemoService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var legalMemoRepository = new LegalMemoRepository(Db, Mapper, UserProvider);
            var legalMemosHistoryRepository = new LegalMemosHistoryRepository(Db, Mapper, UserProvider);
            var legalMemoNoteRepository = new LegalMemoNoteRepository(Db, Mapper, UserProvider);

            return new LegalMemoService(legalMemoRepository, legalMemosHistoryRepository, legalMemoNoteRepository, new Mock<ILogger<LegalMemoService>>().Object);
        }

        public CityService CreateCityService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var cityRepository = new CityRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);
            return new CityService(cityRepository, new Mock<ILogger<CityService>>().Object);
        }

        public MainCategoryService CreateMainCategoryService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var mainCategoryRepository = new MainCategoryRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);

            return new MainCategoryService(mainCategoryRepository, new Mock<ILogger<MainCategoryService>>().Object);
        }

        public FirstSubCategoryService CreateFirstSubCategoryService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var firstSubCategoryRepository = new FirstSubCategoryRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);

            return new FirstSubCategoryService(firstSubCategoryRepository, new Mock<ILogger<FirstSubCategoryService>>().Object);
        }

        public SecondSubCategoryService CreateSecondSubCategoryService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var secondSubCategoryRepository = new SecondSubCategoryRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);
            var mainCategoryService = CreateMainCategoryService();
            var firstSubCategoryService = CreateFirstSubCategoryService();
            var legalMemoService = CreateLegalMemoService();
            var caseServie = CreateCaseService();
            return new SecondSubCategoryService(secondSubCategoryRepository, firstSubCategoryService, mainCategoryService, legalMemoService, caseServie, new Mock<ILogger<SecondSubCategoryService>>().Object);
        }

        public BranchService CreateGeneralManagementService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var generalManagementRepository = new BranchRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);
            return new BranchService(generalManagementRepository, new Mock<ILogger<BranchService>>().Object);
        }

        public IdentityTypeService CreateIdentityTypeService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var identityTypeRepository = new IdentityTypeRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);
            return new IdentityTypeService(identityTypeRepository, new Mock<ILogger<IdentityTypeService>>().Object);
        }

        public JobTitleService CreateJobTitleService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);
            var jobTitleRepository = new JobTitleRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);
            var userService = CreateUserService();
            return new JobTitleService(jobTitleRepository, userService, new Mock<ILogger<JobTitleService>>().Object);
        }

        public ProvinceService CreateProvinceService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var provinceRepository = new ProvinceRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);
            return new ProvinceService(provinceRepository, new Mock<ILogger<ProvinceService>>().Object);
        }

        public AttachmentTypeService CreateAttachmentTypeService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var attachmentTypeRepository = new AttachmentTypeRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);
            return new AttachmentTypeService(attachmentTypeRepository, new Mock<ILogger<AttachmentTypeService>>().Object);
        }

        public LegalBoardService CreateLegalBoardService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var userService = CreateUserService();
            var legalBoardRepository = new LegalBoardRepository(Db, Mapper, UserProvider);
            var boardMeetingRepository = new BoardMeetingRepository(Db, Mapper, UserProvider);
            var notificationService = CreateNotificationService();


            return new LegalBoardService(legalBoardRepository, new Mock<ILogger<LegalBoardService>>().Object, userService, boardMeetingRepository, notificationService);
        }


        public RoleService CreateRoleService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var roleRepository = new RoleRepository(RoleManager, Db, Mapper, UserProvider);

            return new RoleService(roleRepository, new Mock<ILogger<RoleService>>().Object);
        }

        public UserService CreateUserService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var userRepository = new UserRepository(UserManager, Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);
            var notificationService = CreateNotificationService();

            return new UserService(userRepository, new Mock<ILogger<UserService>>().Object, new Mock<IWebHostEnvironment>().Object, notificationService, new Mock<IDistributedCache>().Object, UserProvider);
        }


        #region Workflow

        public WorkflowConfigurationService CreateWorkflowConfigurationService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var workflowConfigurationRepository = new WorkflowConfigurationRepository(Db, Mapper, UserProvider);
            return new WorkflowConfigurationService(workflowConfigurationRepository, new Mock<ILogger<WorkflowConfigurationService>>().Object);
        }

        public WorkflowLookupsService CreateWorkflowLookupsService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var workflowLookupsRepository = new WorkflowLookupsRepository(Db, Mapper, UserProvider);
            return new WorkflowLookupsService(workflowLookupsRepository, new Mock<ILogger<WorkflowLookupsService>>().Object);
        }

        #endregion

        #region Requests
        public HearingLegalMemoService CreateHearingLegalMemoService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);
            var hearingLegalMemoRepository = new HearingLegalMemoRepository(Db, Mapper, UserProvider);
            return new HearingLegalMemoService(hearingLegalMemoRepository, new Mock<ILogger<HearingLegalMemoService>>().Object);
        }
        public AddingLegalMemoToHearingRequestService CreateAddingLegalMemoToHearingRequestService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var addingLegalMemoToHearingRequestRepository = new AddingLegalMemoToHearingRequestRepository(Db, Mapper, UserProvider);
            var researcherConsultantService = CreateResearchsConsultantService();
            var requestService = CreateRequestService();
            var hearingLegalMemoService = CreateHearingLegalMemoService();

            return new AddingLegalMemoToHearingRequestService(addingLegalMemoToHearingRequestRepository, requestService,
                hearingLegalMemoService, new Mock<ILogger<AddingLegalMemoToHearingRequestService>>().Object, researcherConsultantService);
        }

        public ChangeResearcherRequestService CreateChangeResearcherRequestService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var changeResearcherRequestRepository = new ChangeResearcherRequestRepository(Db, Mapper, UserProvider);
            var requestService = CreateRequestService();
            var caseResearchersService = CreateCaseResearcherService();
            var caseService = CreateCaseService();

            return new ChangeResearcherRequestService(changeResearcherRequestRepository, caseResearchersService, requestService,
                caseService, new Mock<ILogger<ChangeResearcherRequestService>>().Object);
        }

        public ChangeResearcherToHearingRequestService CreateChangeResearcherToHearingRequestService(Guid userId = default)
        {
            UpdateLoggedInUser(TestUsers.LegalResearcherId);

            var changeResearcherToHearingRequestRepository = new ChangeResearcherToHearingRequestRepository(Db, Mapper, UserProvider);
            var requestService = CreateRequestService();
            var hearingService = CreateHearingService();
            var userService = CreateUserService();
            var notificationService = CreateNotificationService();

            return new ChangeResearcherToHearingRequestService(changeResearcherToHearingRequestRepository, hearingService, requestService,
                userService, notificationService, new Mock<ILogger<ChangeResearcherToHearingRequestService>>().Object);
        }

        public CaseDocumentRequestService CreateDocumentRequestService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);
            var documentRequestRepository = new CaseSupportingDocumentRequestRepository(Db, Mapper, UserProvider);
            var requestService = CreateRequestService();
            var caseSupportingDocumentRequestHistoryService = CreateCaseSupportingDocumentRequestHistoryService();

            return new CaseDocumentRequestService(documentRequestRepository, requestService, caseSupportingDocumentRequestHistoryService, new Mock<ILogger<CaseDocumentRequestService>>().Object);
        }

        public CaseSupportingDocumentRequestHistoryService CreateCaseSupportingDocumentRequestHistoryService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);
            var caseSupportingDocumentRequestHistoryRepository = new CaseSupportingDocumentRequestHistoryRepository(Db, Mapper, UserProvider);
            return new CaseSupportingDocumentRequestHistoryService(caseSupportingDocumentRequestHistoryRepository, new Mock<ILogger<CaseSupportingDocumentRequestHistoryService>>().Object);
        }
        public RequestMoamalatService CreateRequestMoamalatService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);
            var requestMoamalatRepository = new RequestMoamalatRepository(Mapper, Db, UserProvider);
            var requestService = CreateRequestService();
            var moamalaService = CreateMoamalaService();

            return new RequestMoamalatService(requestMoamalatRepository, requestService, moamalaService, new Mock<ILogger<RequestMoamalatService>>().Object);
        }
        public ExportCaseJudgmentRequestHistoryService CreateExportCaseJudgmentRequestHistoryService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);
            var exportCaseJudgmentRequestHistoryRepository = new ExportCaseJudgmentRequestHistoryRepository(Db, Mapper, UserProvider);
            return new ExportCaseJudgmentRequestHistoryService(exportCaseJudgmentRequestHistoryRepository, new Mock<ILogger<ExportCaseJudgmentRequestHistoryService>>().Object);
        }
        public RequestService CreateRequestService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);
            var requestRepository = new RequestRepository(Db, Mapper, UserProvider);
            var requestTransactionRepository = new RequestTransactionRepository(Mapper, Db, UserProvider);
            var userRepository = new UserRepository(UserManager, Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);
            return new RequestService(requestRepository, requestTransactionRepository, userRepository, new Mock<ILogger<RequestService>>().Object);
        }

        public ExportCaseJudgmentRequestService CreateExportCaseJudgmentRequestService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var exportCaseJudgmentRequestRepository = new ExportCaseJudgmentRequestRepository(Db, Mapper, UserProvider);
            var requestTransactionService = CreateRequestService();
            var exportCaseJudgmentRequestHistoryService = CreateExportCaseJudgmentRequestHistoryService();

            return new ExportCaseJudgmentRequestService(exportCaseJudgmentRequestRepository, requestTransactionService, exportCaseJudgmentRequestHistoryService, new Mock<ILogger<ExportCaseJudgmentRequestService>>().Object);
        }
        public ObjectionPermitRequestService CreateObjectionPermitRequestService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var objectionPermitRequestRepository = new ObjectionPermitRequestRepository(Db, Mapper, UserProvider);
            var requestService = CreateRequestService();
            var caseService = CreateCaseService();
            var userService = CreateUserService();
            var notificationService = CreateNotificationService();
            var hearingRepo = new HearingRepository(Db, Mapper, UserProvider);

            var exportCaseJudgmentRequestHistoryService = CreateExportCaseJudgmentRequestHistoryService();

            return new ObjectionPermitRequestService(objectionPermitRequestRepository, requestService, caseService,
                userService, notificationService, hearingRepo, new Mock<ILogger<ObjectionPermitRequestService>>().Object);
        }

        #endregion

        #region Case
        public CaseService CreateCaseService(Guid userId = default)
        {
            UpdateLoggedInUser(TestUsers.LegalResearcherId);

            var addingObjectionLegalMemoToCaseRequestRepository = new AddingObjectionLegalMemoToCaseRequestRepository(Db, Mapper, UserProvider);
            var objectionPermitRequestRepository = new ObjectionPermitRequestRepository(Db, Mapper, UserProvider);
            var caseRepository = new CaseRepository(Db, Mapper, UserProvider);
            var caseTransactionRepository = new CaseTransactionRepository(Db, Mapper, UserProvider);
            var hearingRepository = new HearingRepository(Db, Mapper, UserProvider);
            var caseResearchersService = CreateCaseResearcherService();
            var notificationService = CreateNotificationService();
            var researcherConsultantService = CreateResearchsConsultantService();
            var userService = CreateUserService();
            var attachmentService = CreateAttachmentService();

            return new CaseService(addingObjectionLegalMemoToCaseRequestRepository, objectionPermitRequestRepository, caseRepository, caseTransactionRepository, hearingRepository, caseResearchersService,
                notificationService, researcherConsultantService, userService, attachmentService, new Mock<ILogger<CaseService>>().Object);
        }

        public CaseResearcherService CreateCaseResearcherService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var caseResearchersRepository = new CaseResearchersRepository(Db, Mapper, UserProvider);

            return new CaseResearcherService(caseResearchersRepository, new Mock<ILogger<CaseResearcherService>>().Object);
        }

        public ResearchsConsultantService CreateResearchsConsultantService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);
            var researcherConsultantRepository = new ResearcherConsultantRepository(Db, Mapper, UserProvider);
            var userService = CreateUserService();

            return new ResearchsConsultantService(researcherConsultantRepository, userService, new Mock<ILogger<ResearchsConsultantService>>().Object);
        }



        #endregion

        public HearingService CreateHearingService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var hearingRepository = new HearingRepository(Db, Mapper, UserProvider);
            var caseService = CreateCaseService();
            var hearingUpdateService = CreateHearingUpdateService();
            var researchsConsultantService = CreateResearchsConsultantService();
            var notificationService = CreateNotificationService();
            var attachmentService = CreateAttachmentService();
            var caseResearcherService = CreateCaseResearcherService();
            var userService = CreateUserService();
            var options = new Mock<IOptionsSnapshot<AppSettings>>();

            return new HearingService(hearingRepository, caseService,
                hearingUpdateService, notificationService, researchsConsultantService, caseResearcherService, userService, attachmentService, options.Object, new Mock<ILogger<HearingService>>().Object);
        }

        public HearingUpdateService CreateHearingUpdateService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var hearingUpdateRepository = new HearingUpdateRepository(Db, Mapper, UserProvider);
            var attachmentService = CreateAttachmentService();

            return new HearingUpdateService(hearingUpdateRepository, attachmentService, new Mock<ILogger<HearingUpdateService>>().Object);
        }

        public AttachmentService CreateAttachmentService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var attachmentRepository = new AttachmentRepository(Db, Mapper, UserProvider);

            return new AttachmentService(attachmentRepository, new Mock<ILogger<AttachmentService>>().Object);
        }

        public InvestigationService CreateInvestigationService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var investigationRepository = new InvestigationRepository(Db, Mapper, UserProvider);
            var investigationQuestionRepository = new InvestigationQuestionRepository(Db, Mapper, UserProvider, new Mock<IDistributedCache>().Object);

            return new InvestigationService(investigationRepository, new Mock<ILogger<InvestigationService>>().Object);
        }

        public MoamalaService CreateMoamalaService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var moamalaRepository = new MoamalaRepository(Db, Mapper, UserProvider);
            var moamalaTransactionRepository = new MoamalaTransactionRepository(Db, Mapper, UserProvider);
            var attachmentService = CreateAttachmentService();
            var userService = CreateUserService();

            return new MoamalaService(moamalaRepository, moamalaTransactionRepository, attachmentService, new Mock<ILogger<MoamalaService>>().Object, userService);
        }

        public ConsultationService CreateConsultationService(Guid userId = default)
        {
            UpdateLoggedInUser(TestUsers.LegalResearcherId);
            var consultationRepository = new ConsultationRepository(Db, Mapper, UserProvider);
            var consultationTransactionService = CreateConsultationTransactionService();
            return new ConsultationService(consultationRepository, consultationTransactionService, new Mock<ILogger<ConsultationService>>().Object);
        }
        public ConsultationTransactionService CreateConsultationTransactionService(Guid userId = default)
        {
            UpdateLoggedInUser(userId);

            var consultationTransactionRepository = new ConsultationTransactionRepository(Mapper, Db, UserProvider);
            return new ConsultationTransactionService(consultationTransactionRepository, new Mock<ILogger<ConsultationTransactionService>>().Object);
        }
        public NotificationService CreateNotificationService(Guid userId = default)
        {
            var notificationRepository = new NotificationRepository(Db, Mapper, UserProvider);

            return new NotificationService(notificationRepository, new Mock<ILogger<NotificationService>>().Object);
        }

        public NotificationSystemService CreateNotificationSystemService(Guid userId = default)
        {
            var notificationSystemRepository = new NotificationSystemRepository(Db, Mapper, UserProvider);

            return new NotificationSystemService(notificationSystemRepository, new Mock<ILogger<NotificationSystemService>>().Object);
        }

        public NotificationSMSService CreateNotificationSMSService(Guid userId = default)
        {
            var notificationSMSRepository = new NotificationSMSRepository(Db, Mapper, UserProvider);
            var moeSmsIntegrationService = new MoeSmsIntegrationService(new Mock<IOptions<MoeSmsOptions>>().Object, new Mock<ILogger<MoeSmsIntegrationService>>().Object);

            return new NotificationSMSService(notificationSMSRepository, moeSmsIntegrationService, new Mock<ILogger<NotificationSMSService>>().Object);
        }

        public MoeEmailIntegrationService CreateMOEEmailIntegrationService(Guid userId = default)
        {
            return new MoeEmailIntegrationService(new Mock<IOptions<MoeEmailOptions>>().Object, new Mock<ILogger<MoeEmailIntegrationService>>().Object);
        }

        public NotificationEmailService CreateNotificationEmailService(Guid userId = default)
        {
            var notificationEmailRepository = new NotificationEmailRepository(Db, Mapper, UserProvider);
            var moeEmailIntegrationService = CreateMOEEmailIntegrationService();
            return new NotificationEmailService(notificationEmailRepository, moeEmailIntegrationService, new Mock<ILogger<NotificationEmailService>>().Object);
        }

        private void UpdateLoggedInUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return;
            }

            var userProvider = new Mock<IUserProvider>();
            var currentUser = new CurrentUser
            {
                UserId = userId
            };
            userProvider.Setup(m => m.CurrentUser).Returns(currentUser);

            UserProvider = userProvider.Object;
        }
    }
}
