using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moe.La.Core.Entities;
using Moe.La.Core.Entities.Integration.Moeen;
using Moe.La.Core.Entities.Litigation.BoardMeeting;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.DbContexts
{
    public class LaDbContext : IdentityDbContext<
        AppUser, AppRole, Guid,
        AppUserClaim, AppUserRole, AppUserLogin,
        AppRoleClaim, AppUserToken>
    {

        private IDbContextTransaction _currentTransaction;

        // options added in the Startup=> ConfigureServices and will be based to the constructor of IdentityDbContext
        // options conatains the Database type and ConnectionString

        public LaDbContext(DbContextOptions<LaDbContext> options) : base(options)
        {
        }

        public DbSet<WorkflowAction> WorkflowActions { get; set; }
        public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
        public DbSet<WorkflowInstanceLog> WorkflowInstanceLogs { get; set; }
        public DbSet<WorkflowStatus> WorkflowStatuses { get; set; }
        public DbSet<WorkflowStepAction> WorkflowStepsActions { get; set; }
        public DbSet<WorkflowStepCategory> WorkflowStepsCategories { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }
        public DbSet<WorkflowStepPermission> WorkflowStepsPermissions { get; set; }
        public DbSet<WorkflowType> WorkflowTypes { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AttachmentType> AttachmentsTypes { get; set; }
        public DbSet<CaseAttachment> CaseAttachments { get; set; }
        public DbSet<CaseRuleAttachment> CaseRuleAttachments { get; set; }
        public DbSet<HearingAttachment> HearingAttachments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationSystem> NotificationSystems { get; set; }
        public DbSet<NotificationSMS> NotificationSMSs { get; set; }
        public DbSet<NotificationEmail> NotificationEmails { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<CaseMoamala> CaseMoamalat { get; set; }
        public DbSet<CaseTransaction> CaseTransactions { get; set; }
        public DbSet<MoamalaTransaction> MoamalaTransactions { get; set; }
        public DbSet<CaseGrounds> CaseGrounds { get; set; }
        public DbSet<CaseResearcher> CaseResearchers { get; set; }
        public DbSet<CaseRule> CaseRules { get; set; }
        public DbSet<Hearing> Hearings { get; set; }
        public DbSet<PartyType> PartyTypes { get; set; }
        public DbSet<IdentityType> IdentityTypes { get; set; }
        public DbSet<FieldMissionType> FieldMissionTypes { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<CaseParty> CaseParties { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<GovernmentOrganization> GovernmentOrganizations { get; set; }
        public DbSet<LegalMemo> LegalMemos { get; set; }
        public DbSet<LegalMemoHistory> LegalMemosHistory { get; set; }
        public DbSet<ResearcherConsultant> ResearcherConsultants { get; set; }
        public DbSet<ResearcherConsultantHistory> ResearcherConsultantsHistory { get; set; }
        public DbSet<LegalBoard> LegalBoards { get; set; }
        public DbSet<LegalMemoNote> LegalMemoNotes { get; set; }
        public DbSet<LegalBoardMember> LegalBoardMembers { get; set; }
        public DbSet<LegalBoardMemberHistory> LegalBoardMemberHistory { get; set; }
        public DbSet<LegalBoardMemo> LegalBoardMemos { get; set; }
        public DbSet<BoardMeeting> BoardMeetings { get; set; }
        public DbSet<BoardMeetingMember> BoardMeetingMembers { get; set; }
        public DbSet<Core.Entities.Request> Requests { get; set; }
        public DbSet<ChangeResearcherRequest> ChangeResearcherRequests { get; set; }
        public DbSet<ChangeResearcherToHearingRequest> ChangeResearcherToHearingRequests { get; set; }
        public DbSet<ConsultationSupportingDocumentRequest> ConsultationSupportingDocuments { get; set; }
        public DbSet<CaseSupportingDocumentRequest> DocumentRequests { get; set; }
        public DbSet<CaseSupportingDocumentRequestItem> DocumentRequestItems { get; set; }
        public DbSet<RequestHistory> RequestHistory { get; set; }
        public DbSet<CaseSupportingDocumentRequestHistory> CaseSupportingDocumentRequestHistory { get; set; }
        public DbSet<ExportCaseJudgmentRequestHistory> ExportCaseJudgmentRequestHistory { get; set; }
        public DbSet<CaseSupportingDocumentRequestItemHistory> CaseSupportingDocumentRequestItemHistory { get; set; }
        public DbSet<ExportCaseJudgmentRequest> ExportCaseJudgmentRequests { get; set; }
        public DbSet<HearingLegalMemo> HearingLegalMemos { get; set; }
        public DbSet<AddingLegalMemoToHearingRequest> HearingLegalMemoReviewRequests { get; set; }
        public DbSet<RequestTransaction> RequestTransactions { get; set; }
        public DbSet<MoamalaRasel> MoamalatRasel { get; set; }
        public DbSet<MoamalaTransaction> MoamalatTransactions { get; set; }
        public DbSet<MoeenCase> MoeenCases { get; set; }
        public DbSet<NajizCase> NajizCases { get; set; }
        public DbSet<SecondSubCategory> SecondSubCategories { get; set; }
        public DbSet<FirstSubCategory> FirstSubCategories { get; set; }
        public DbSet<MainCategory> MainCategories { get; set; }
        public DbSet<HearingUpdate> HearingUpdates { get; set; }
        public DbSet<HearingUpdateAttachment> HearingUpdateAttachment { get; set; }
        public DbSet<RequestNote> RequestNotes { get; set; }
        public DbSet<PartyEntityType> PartyEntityTypes { get; set; }
        public DbSet<CaseRuleMinistryDepartment> CaseRuleMinistryDepartments { get; set; }
        public DbSet<MinistryDepartment> MinistryDepartments { get; set; }
        public DbSet<MinistrySector> MinistrySectors { get; set; }
        public DbSet<MinistryDepartment> CaseRuleProsecutorRequests { get; set; }
        public DbSet<Investigation> Investigations { get; set; }
        public DbSet<InvestigationRecord> InvestigationRecords { get; set; }
        public DbSet<InvestigationRecordParty> InvestigationRecordParties { get; set; }
        public DbSet<InvestigationRecordQuestion> InvestigationRecordQuestions { get; set; }
        public DbSet<InvestigationRecordPartyType> InvestigationRecordPartyTypes { get; set; }
        public DbSet<InvestigationQuestion> InvestigationQuestions { get; set; }
        public DbSet<InvestigationRecordAttachment> InvestigationRecordAttachments { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<EducationalLevel> EducationalLevels { get; set; }
        public DbSet<InvestigationPartyPenalty> InvestigationPartyPenalties { get; set; }
        public DbSet<Moamala> Moamalat { get; set; }
        public DbSet<MoamalaNotification> MoamalatNotifications { get; set; }
        public DbSet<MoamalaAttachment> MoamalaAttachments { get; set; }
        public DbSet<WorkItemType> WorkItemTypes { get; set; }
        public DbSet<SubWorkItemType> SubWorkItemTypes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<RequestsMoamalat> RequestsMoamalat { get; set; }
        public DbSet<BranchesDepartments> BranchDepartments { get; set; }
        public DbSet<UserRoleDepartment> AppUserRoleDepartmets { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<ConsultationMoamalat> ConsultationMoamalat { get; set; }
        public DbSet<ConsultationMerits> ConsultationMerits { get; set; }
        public DbSet<ConsultationGrounds> ConsultationGrounds { get; set; }
        public DbSet<ConsultationTransaction> ConsultationTransactions { get; set; }
        public DbSet<ConsultationVisual> ConsultationVisuals { get; set; }
        public DbSet<LetterTemplate> LetterTemplates { get; set; }
        public DbSet<RequestLetter> RequestLetters { get; set; }
        public DbSet<CaseHistory> CaseHistory { get; set; }
        public DbSet<ObjectionPermitRequest> ObjectionPermitRequests { get; set; }
        public DbSet<AddingObjectionLegalMemoToCaseRequest> AddingObjectionLegalMemoToCaseRequests { get; set; }

        #region Moeen
        public DbSet<InformLetter> InformLetters { get; set; }
        public DbSet<Defendant> Defendants { get; set; }
        public DbSet<Prosecutor> Prosecutors { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<ProsecutorRequest> ProsecutorRequests { get; set; }
        public DbSet<Core.Entities.Integration.Moeen.Request> MoeenRequests { get; set; }
        public DbSet<Ruling> Rulings { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);
                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
