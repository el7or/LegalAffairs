using Microsoft.Extensions.DependencyInjection;
using Moe.La.Common.Resources;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.Repositories;
using Moe.La.Integration;
using Moe.La.Integration.Interfaces;
using Moe.La.ServiceInterface;

namespace Moe.La.WebApi
{
    public static class ServiceCollectionSetup
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICaseService, CaseService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IMinistryDepartmentService, MinistryDepartmentService>();
            services.AddScoped<IMinistrySectorService, MinistrySectorService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IPartyService, PartyService>();
            services.AddScoped<IRoleClaimService, RoleClaimService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IResearchsConsultantService, ResearchsConsultantService>();
            services.AddScoped<ILegalMemoService, LegalMemoService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IChangeResearcherRequestService, ChangeResearcherRequestService>();
            services.AddScoped<IChangeResearcherToHearingRequestService, ChangeResearcherToHearingRequestService>();
            services.AddScoped<ICaseSupportingDocumentRequestService, CaseDocumentRequestService>();
            services.AddScoped<IAddingLegalMemoToHearingRequestService, AddingLegalMemoToHearingRequestService>();
            services.AddScoped<IAttachmentTypeService, AttachmentTypeService>();
            services.AddScoped<ICourtService, CourtService>();
            services.AddScoped<IIdentityTypeService, IdentityTypeService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IJobTitleService, JobTitleService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationSystemService, NotificationSystemService>();
            services.AddScoped<IHearingService, HearingService>();
            services.AddScoped<IHearingUpdateService, HearingUpdateService>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IInvestigationRecordPartyTypeService, InvestigationRecordPartyTypeService>();
            services.AddScoped<ILegalBoardService, LegalBoardService>();
            services.AddScoped<IMoeenCaseLogService, MoeenCaseLogService>();
            services.AddScoped<IWorkflowConfigurationService, WorkflowConfigurationService>();
            services.AddScoped<IWorkflowLookupsService, WorkflowLookupsService>();
            services.AddScoped<IMoamalatRaselService, MoamalatRaselService>();
            services.AddScoped<IExportCaseJudgmentRequestService, ExportCaseJudgmentRequestService>();
            services.AddScoped<IConsultationSupportingDocumentRequestService, ConsultationSupportingDocumentService>();
            services.AddScoped<INajizCaseLogService, NajizCaseLogService>();
            services.AddScoped<IInvestigationRecordService, InvestigationRecordService>();
            services.AddScoped<IInvestigationQuestionService, InvestigationQuestionService>();
            services.AddScoped<IInvestigationService, InvestigationService>();
            services.AddScoped<IFarisIntegrationService, FarisIntegrationService>();
            services.AddScoped<INoorIntegrationService, NoorIntegrationService>();
            services.AddScoped<IInvestiationRecordPartyService, InvestiationRecordPartyService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMoamalaService, MoamalaService>();
            services.AddScoped<IRequestMoamalatService, RequestMoamalatService>();
            services.AddScoped<IWorkItemTypeService, WorkItemTypeService>();
            services.AddScoped<ISubWorkItemTypeService, SubWorkItemTypeService>();
            services.AddScoped<IConsultationService, ConsultationService>();
            services.AddScoped<IConsultationTransactionService, ConsultationTransactionService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IGovernmentOrganizationService, GovernmentOrganizationService>();
            services.AddScoped<ICaseResearcherService, CaseResearcherService>();
            services.AddScoped<ICaseSupportingDocumentRequestHistoryService, CaseSupportingDocumentRequestHistoryService>();
            services.AddScoped<IHearingLegalMemoService, HearingLegalMemoService>();
            services.AddScoped<IExportCaseJudgmentRequestHistoryService, ExportCaseJudgmentRequestHistoryService>();
            services.AddScoped<IMainCategoryService, MainCategoryService>();
            services.AddScoped<IFirstSubCategoryService, FirstSubCategoryService>();
            services.AddScoped<ISecondSubCategoryService, SecondSubCategoryService>();
            services.AddScoped<ILetterTemplateService, LetterTemplateService>();
            services.AddScoped<IRequestLetterService, RequestLetterService>();
            services.AddScoped<IObjectionPermitRequestService, ObjectionPermitRequestService>();
            services.AddScoped<IAddingObjectionLegalMemoToCaseRequestService, AddingObjectionLegalMemoToCaseRequestService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IResearcherConsultantRepository, ResearcherConsultantRepository>();
            services.AddScoped<IRoleClaimRepository, RoleClaimRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationSystemRepository, NotificationSystemRepository>();
            services.AddScoped<IJobTitleRepository, JobTitleRepository>();
            services.AddScoped<ICourtRepository, CourtRepository>();
            services.AddScoped<IPartyRepository, PartyRepository>();
            services.AddScoped<ILegalMemoRepository, LegalMemoRepository>();
            services.AddScoped<ILegalMemosHistoryRepository, LegalMemosHistoryRepository>();
            services.AddScoped<ILegalMemoNoteRepository, LegalMemoNoteRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IChangeResearcherRequestRepository, ChangeResearcherRequestRepository>();
            services.AddScoped<IChangeResearcherToHearingRequestRepository, ChangeResearcherToHearingRequestRepository>();
            services.AddScoped<ICaseSupportingDocumentRequestRepository, CaseSupportingDocumentRequestRepository>();
            services.AddScoped<ICaseSupportingDocumentRequestHistoryRepository, CaseSupportingDocumentRequestHistoryRepository>();
            services.AddScoped<IAddingLegalMemoToHearingRequestRepository, AddingLegalMemoToHearingRequestRepository>();
            services.AddScoped<ICaseSupportingDocumentRequestItemRepository, CaseSupportingDocumentRequestItemRepository>();
            services.AddScoped<ICaseRepository, CaseRepository>();
            services.AddScoped<ICaseTransactionRepository, CaseTransactionRepository>();
            services.AddScoped<IHearingRepository, HearingRepository>();
            services.AddScoped<IMinistryDepartmentRepository, MinistryDepartmentRepository>();
            services.AddScoped<IMinistrySectorRepository, MinistrySectorRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IIdentityTypeRepository, IdentityTypeRepository>();
            services.AddScoped<IAttachmentTypeRepository, AttachmentTypeRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IInvestigationRecordPartyTypeRepository, InvestigationRecordPartyTypeRepository>();
            services.AddScoped<ILegalBoardRepository, LegalBoardRepository>();
            services.AddScoped<IBoardMeetingRepository, BoardMeetingRepository>();
            services.AddScoped<IMoeenCaseLogRepository, MoeenCaseLogRepository>();
            services.AddScoped<IWorkflowConfigurationRepository, WorkflowConfigurationRepository>();
            services.AddScoped<ICaseResearchersRepository, CaseResearchersRepository>();
            services.AddScoped<IHearingLegalMemoRepository, HearingLegalMemoRepository>();
            services.AddScoped<IWorkflowLookupsRepository, WorkflowLookupsRepository>();
            services.AddScoped<IRequestTransactionRepository, RequestTransactionRepository>();
            services.AddScoped<IExportCaseJudgmentRequestHistoryRepository, ExportCaseJudgmentRequestHistoryRepository>();
            services.AddScoped<IMoamalaRaselRepository, MoamalaRaselRepository>();
            services.AddScoped<IMoamalaTransactionRepository, MoamalaTransactionRepository>();
            services.AddScoped<IExportCaseJudgmentRequestRepository, ExportCaseJudgmentRequestRepository>();
            services.AddScoped<IConsultationSupportingDocumentRequestRepository, ConsultationSupportingDocumentRepository>();
            services.AddScoped<INajizCaseLogRepository, NajizCaseLogRepository>();
            services.AddScoped<IHearingUpdateRepository, HearingUpdateRepository>();
            services.AddScoped<IInvestigationQuestionRepository, InvestigationQuestionRepository>();
            services.AddScoped<IInvestigationRecordRepository, InvestigationRecordRepository>();
            services.AddScoped<IInvestiationRecordPartyRepository, InvestiationRecordPartyRepository>();
            services.AddScoped<IInvestigationRepository, InvestigationRepository>();
            services.AddScoped<IMoamalaRepository, MoamalaRepository>();
            services.AddScoped<IRequestMoamalatRepository, RequestMoamalatRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IWorkItemTypeRepository, WorkItemTypeRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ISubWorkItemTypeRepository, SubWorkItemTypeRepository>();
            services.AddScoped<IConsultationRepository, ConsultationRepository>();
            services.AddScoped<IConsultationTransactionRepository, ConsultationTransactionRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IGovernmentOrganizationRepository, GovernmentOrganizationRepository>();
            services.AddScoped<IMainCategoryRepository, MainCategoryRepository>();
            services.AddScoped<IFirstSubCategoryRepository, FirstSubCategoryRepository>();
            services.AddScoped<ISecondSubCategoryRepository, SecondSubCategoryRepository>();
            services.AddScoped<ILetterTemplateRepository, LetterTemplateRepository>();
            services.AddScoped<IRequestLetterRepository, RequestLetterRepository>();
            services.AddScoped<IObjectionPermitRequestRepository, ObjectionPermitRequestRepository>();
            services.AddScoped<IAddingObjectionLegalMemoToCaseRequestRepository, AddingObjectionLegalMemoToCaseRequestRepository>();
        }

        public static void AddIntegrationServices(this IServiceCollection services)
        {
            services.AddScoped<IMoeEmailIntegrationService, MoeEmailIntegrationService>();
        }

        public static void AddLocalizationResources(this IServiceCollection services)
        {
            services.AddSingleton<Localization<Messages>>();
        }
    }
}

