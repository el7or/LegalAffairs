import { AppBreadcrumbComponent } from 'app/layouts/full/breadcrumb/breadcrumb.component';
import { LegalMemoStatus } from '../enums/LegalMemoStatus';
import { LegalMemoTypes } from '../enums/LegalMemoTypes';
import { GroupNames } from './attachment';

export class QueryObject {
  sortBy?: string = 'name';
  isSortAscending?: boolean = true;
  page: number = 1;
  pageSize: number = 20;
  public constructor(init?: Partial<QueryObject>) {
    Object.assign(this, init);
  }
}
export class QueryResult {
  totalItems: number = 0;
  items: any[] = [];
}

export class RequestQueryObject extends QueryObject {
}

export class LegalBoardMemberQueryObject extends QueryObject {
  userId?: string;
  legalBoardId?: number;
  public constructor(init?: Partial<MemoQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}

export interface BranchQueryObject extends QueryObject {
  isParent?: boolean;
  name?: string;
}

export class CaseCategoryQueryObject extends QueryObject {
  public constructor(init?: Partial<CaseCategoryQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
  caseSource?: number;
}

export class PartyQueryObject extends QueryObject {
  public constructor(init?: Partial<PartyQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
  partyType?: number;
  identityTypeId?: number;
  provinceId?: number;
  cityId?: number;
  name?: string;
  companyName?: string;
  partyTypeName?: string;
  identityValue?: string;
}

export class MainCategoryQueryObject extends QueryObject {
  public constructor(init?: Partial<MainCategoryQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
  caseSource?: number;
}

export class FirstSubCategoryQueryObject extends QueryObject {
  public constructor(init?: Partial<FirstSubCategoryQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
  mainCategoryId?: number;
}

export class SecondSubCategoryQueryObject extends QueryObject {
  public constructor(init?: Partial<SecondSubCategoryQueryObject>) {
    super(init);
    Object.assign(this, init);
  }

  firstSubCategoryId?: number;
  isActive?: boolean;
}


export class NotificationQueryObject extends QueryObject {
  text?: string = '';
}

export class MemoQueryObject extends QueryObject {
  name?: string = '';
  secondSubCategoryId?: number;
  status: LegalMemoStatus[] = [];
  type: LegalMemoTypes;
  createdBy?: string = '';
  createdOn?: string = '';
  updatedOn?: string = '';
  approvalFrom?: string = '';
  approvalTo?: string = '';
  raisedFrom?: string = '';
  raisedTo?: string = '';
  searchText?: string = '';
  isReview?: boolean = false;
  isBoardReview?: boolean = false;
  initialCaseId?: number;
  public constructor(init?: Partial<MemoQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}

export class LegalMemoNoteQueryObject extends QueryObject {
  legalMemoId?: number;
}

export class UserQueryObject extends QueryObject {
  enabled?: boolean;

  roles: string[] = [];

  branchId?: number;

  departmetId?: number;

  fullName?: string;

  email?: string;

  identityNumber?: string;

  searchText?: string;

  hasConfidentialPermission?: boolean;

  workItemTypeId?: number;

  public constructor(init?: Partial<UserQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}


export class ResearcherQueryObject extends QueryObject {
  researcherId?: number;
  consultantId?: number;
  hasConsultant?: boolean;
  isSameBranch: boolean;

  public constructor(init?: Partial<ResearcherQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}
// export interface LegalAffairsDepartmentQueryObject extends QueryObject {
//   sectorId: number;
// }

export interface CityQueryObject extends QueryObject {
  provinceId: number;
}

export interface AdversaryQueryObject extends QueryObject {
  categoryId: number;

  provinceId: number;

  cityId: number;
}

export interface CustomerQueryObject extends QueryObject {
  partyTypeId: number;

  provinceId: number;

  cityId: number;
}

export interface AgencyQueryObject extends QueryObject {
  customerId: number;

  enabled: number;
}

export class CaseQueryObject extends QueryObject {
  startDateFrom?: string;
  startDateTo?: string;
  caseSource?: number;
  legalStatus?: number;
  litigationType?: number;
  receivedStatus?: number;
  isDuplicated?: boolean;
  status?: number;
  courtId?: string[];
  circleNumber?: string;
  addUserId?: string;
  subject?: string;
  sectorId?: number;
  BranchId?: number;
  adversaryId?: number;
  courtTypeId?: number;
  consultantId?: string;
  closeFrom?: string;
  closeTo?: string;
  searchText?: string;
  isFinalJudgment?: boolean;
  isClosedCase?: boolean;
  isCaseDataCompleted?: boolean;
  isForHearingAddition?: boolean;
  isManual?: boolean;
  partyName?: string;
  referenceCaseNo?: string;
  isForChooseRelatedCase?: boolean;
  legalMemoType?: number;

  public constructor(init?: Partial<CaseQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}

export class HearingQueryObject extends QueryObject {
  caseId?: number;
  courtId?: number;
  status?: number;
  hearingNumber?: number;

  consultantId?: string;

  from?: string;

  to?: string;

  closed?: boolean;

  searchText?: string;
}

export class HearingUpdateQueryObject extends QueryObject {
  updateDate?: string;
  attachment?: string;
  searchText?: string;
  hearingId: number = 0;
  public constructor(init?: Partial<HearingUpdateQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}
export class CourtQueryObject extends QueryObject {
  courtCategory?: number;
  litigationType?: number;
  public constructor(init?: Partial<CourtQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}

export interface HearingProcedureQueryObject extends QueryObject {
  caseId?: number;

  hearingId: number;

  typeId: number[];

  addDate: string;

  supervisorId?: string;

  consultantId?: string;

  currentUserId?: string;

  isReplied?: boolean;

  from?: string;

  to?: string;

  hearingClosed?: boolean;

  querySummary?: string;
}

export interface HearingRequestFileQueryObject extends QueryObject {
  hearingId: number;

  caseId: null;

  currentUserId?: string;

  closed?: boolean;

  from?: string;

  to?: string;

  consultantId?: string;

  hearingClosed?: boolean;
}

export interface NotificationSystemQueryObject extends QueryObject {
  isForCurrentUser?: boolean;

  isRead?: boolean;
}

export interface SecondPartyQueryObject extends QueryObject {
  categoryId: number;

  provinceId: number;

  cityId: number;
}

export interface CaseChangeConsultantRequestQueryObject extends QueryObject {
  caseId: number;
  addUserId: string;
  isAccept?: boolean;
}
export interface TransactionQueryObject extends QueryObject {
  requestType?: number;
  requestStatus?: number;
  requestId: number;
}
export interface ExecutiveCaseQueryObject extends QueryObject {
  addUserId: string;
  adversaryName?: string;
  circleNumber?: string;
  closed?: boolean;
  from: string;
  to: string;
  closedFrom?: string;
  closedTo?: string;
}
export interface PoliceCaseQueryObject extends QueryObject {
  addUserId: string;
  adversaryName?: string;
  departmentName?: string;
  closed?: boolean;
  typeId: number;
  from: string;
  to: string;
  closedFrom?: string;
  closedTo?: string;
}
export interface ExecutiveCaseProcedureQueryObject extends QueryObject {
  executiveCaseId?: number;
  addUserId: string;
}
export interface PoliceCaseProcedureQueryObject extends QueryObject {
  policeCaseId?: number;
  addUserId: string;
}

// export interface ConsultationQueryObject extends QueryObject {
//   addUserId: string;
//   typeId: number;
//   subject: string;
//   sectorId: number;
//   generalManagementId: number;
//   from: string;
//   to: string;
//   stageId: string;
//   consultantId: string;
//   consultantAssignmentDate?: string;
//   querySummary?: string;
// }

export interface ConsultationRequestFileQueryObject extends QueryObject {
  consultationId?: number;
  subject: string;
  assignedUserId?: string;
  closed?: boolean;
  from?: string;
  to?: string;
  consultantId?: string;
  consultationApproved?: boolean;
}

export interface AttachmentQueryObject extends QueryObject {
  /**
   * Case or Hearing or LegalMemo ...etc
   */
  groupName: GroupNames;
  /**
   * CaseId or HearingId or LegalMemoId ...etc
   */
  //groupId: number;
}

export class MoamalaTransactionQueryObject extends QueryObject {
  transactionType?: number;
  subject?: string;
  referenceNo?: string;
  unifiedNo?: string;
  sendingDepartment?: string;
  searchText?: string;
}
export class InvestigationRecordQueryObject extends QueryObject {
  investigationId?: number;
  searchText?: string;
}
export class InvestigationQueryObject extends QueryObject {
  searchText?: string;
}

export class MoamalatQueryObject extends QueryObject {
  searchText?: string;
  status?: number;
  confidentialDegree?: number;
  senderDepartmentId?: number;
  receiverDepartmentId?: number;
  assignedToId?: number;
  createdOnFrom?: string = '';
  createdOnTo?: string = '';
  relatedId?: number;

  public constructor(init?: Partial<MoamalatQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}

export class SubWorkItemTypeQueryObject extends QueryObject {
  workItemTypeId?: number;
  public constructor(init?: Partial<SubWorkItemTypeQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}

export class MoamalatRaselQueryObject extends QueryObject {
  searchText?: string;
  status?: number;
  itemPrivacy?: number;
  createdOnFrom?: string = '';
  createdOnTo?: string = '';

  public constructor(init?: Partial<MoamalatRaselQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}

export class WorkItemTypeQueryObject extends QueryObject {
  departmentId?: number;
  public constructor(init?: Partial<WorkItemTypeQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}

export class ConsultationQueryObject extends QueryObject {
  searchText: string = '';
  departmentId?: number;
  dateFrom?: string;
  dateTo?: string;
  workItemTypeId?: number;
  status?: number;
  assignedTo?: string;
  hasConfidentialAccess: false;
}
export class MinistrySectorQueryObject extends QueryObject {
  ministrySectorId?: number;
}

export class TemplateQueryObject extends QueryObject {
  name: string = '';
  type?: number;
}

export class MeetingQueryObject extends QueryObject {
  meetingPlace?: string;
  meetingDateFrom?: string;
  meetingDateTo?: string;
  searchText?: string;
  public constructor(init?: Partial<MeetingQueryObject>) {
    super(init);
    Object.assign(this, init);
  }
}
