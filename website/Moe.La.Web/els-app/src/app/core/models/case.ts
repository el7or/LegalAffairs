import { KeyValuePairs } from './key-value-pairs';
import { BaseModel } from './base-model';
import { Attachment } from './attachment';
import { CaseRuleDetails } from './case-rule';
import { CasePartyDetails } from './case-party';
import { HearingDetails } from './hearing';

export class CaseListItem extends BaseModel<number> {
  id: number = 0;
  receivedDate: string = ''; // تاريخ الاستلام
  receivedDateHigri: string = ''; // تاريخ الاستلام
  createdOnHigri: string = ''; // تاريخ الاستلام

  receiptStatus: string = ''; // حالة الاستلام
  raselRef: string = '';
  raselUnifiedNo: string = '';
  caseNumberInSource: string = '';//رقم القضية
  najizRef: string = ''; // رقم المعاملة ٝي ناجز

  najizId: string = ''; // رقم الطلب ٝي ناجز
  moeenRef: string = ''; // رقم المعاملة ٝي معين

  caseSource!: KeyValuePairs; //مصدر القضية
  litigationType: string = ''; // درجة التراٝع
  mainNo: string = ''; // الرقم الرئيسي
  startDate: string = ''; // تاريخ بداية القضية
  startDateHigri: string = ''; // تاريخ بداية القضية
  court: string = ''; //المحكمة
  circleNumber: string = ''; //الدائرة
  subject: string = ''; //عنوان الدعوى
  type: string = ''; //صٝة الوزارة القانونية
  caseDescription: string = ''; //موضوع الدعوى
  attachments: string[] = []; // مرٝقات أسانيد الدعوى[قائمة]
  orderDescription: string = ''; //وصٝ الطلب

  isDuplicated?: boolean; // مكررة أم لا
  relatedCaseId?: number; // القضية المرتبطة
  relatedCase!: CaseListItem;
  branch?: string; // الإدارة المحال إليها القضية
  status!: KeyValuePairs; // حالة القضية
  hearingsCount?: number; // عدد الجلسات ٝي القضية

  pronouncingJudgmentDate: string = ''; // تاريخ نطق الحكم
  receivingJudgmentDate: string = '';// موعد استلام الحكم
  caseGrounds: string[] = [];
  pronouncingJudgmentDateHigri?: string = '';
  receivingJudgmentDateHigri?: string = '';
  objectionJudgmentLimitDateHijri?: string = '';
  isAllowObjectionMemo?: boolean = false;
  secondSubCategoryId: number;
  notes: string = ''; // ملاحظات

  caseSourceNumber: string = '';

}

export class CaseDetails extends BaseModel<number> {
  isManual?: boolean;
  raselRef: string = '';
  raselUnifiedNo: string = '';
  najizRef: string = '';
  najizId: string = '';
  moeenRef: string = '';
  caseNumberInSource: string = '';
  parentId?: number;
  caseSource: KeyValuePairs = { id: 0, name: '' };
  litigationType: KeyValuePairs = { id: 0, name: '' };
  referenceCaseNo: string = '';
  mainNo: string = '';
  startDateHigri: string = '';
  startDate!: Date;
  court: KeyValuePairs = { id: 0, name: '' };
  circleNumber: string = '';
  subject: string = '';
  legalStatus: KeyValuePairs = { id: 0, name: '' };
  caseDescription: string = '';
  orderDescription: string = '';
  status: KeyValuePairs = { id: 0, name: '' };
  branchId?: number = 0;
  branchName: string = '';
  recordDate: string = '';
  fileNo: string = '';
  judgeName: string = '';
  closeDate: string = '';
  relatedCaseId?: number;
  relatedCase!: CaseListItem;
  pronouncingJudgmentDate?: Date;
  receivingJudgmentDate?: Date;
  caseRule?: CaseRuleDetails;
  caseRuleId?: number;
  judgementText?: string;
  isFinalJudgment?: boolean;
  judgementResult?: string;
  judgmentBrief?: string;
  attachmentsCount!: number;
  hearings: any[] = null!;
  caseMoamalat?: any[] = [];
  researchers: KeyValuePairs<string>[] = null!;
  parties: any[] = [];
  caseParties?: CasePartyDetails[] = [];
  plaintiffs: any[] = [];
  respondents: any[] = [];
  judgementImportDate?: Date;
  objectionJudgmentLimitDateHijri?: any;
  pronouncingJudgmentDateHijri?: any;
  receivingJudgmentDateHijri?: any;

  caseTransactions?: CaseTransaction[] = [];
  secondSubCategory?: KeyValuePairs = { id: 0, name: '' };
  subCategory?: any;
  caseGrounds: CaseGrounds[] = [];
  prosecutorRequests: any[] = [];
  attachments: any[] = [];
  notes: string = '';
  finishedPronouncedHearing:boolean;
  remainingObjetcion:number=0;
}

export class CaseTransaction extends BaseModel<number> {

  createdByUser: string = '';
  // createdByRole: string = '';
  text: string = '';

}

export class CaseGrounds {
  text: string = '';
}

export class SaveCase {
  id: number = 0;
  raselRef: string = '';
  raselUnifiedNo: string = '';
  caseSourceNumber: string = '';
  najizRef: string = '';
  najizId: string = '';
  moeenRef: string = '';
  caseSource: string = '';
  litigationType: string = '';
  referenceCaseNo: string = '';
  mainNo: string = '';
  startDate!: Date;
  courtId: number = 0
  circleNumber: string = '';
  subject: string = '';
  legalStatus: string = '';
  caseDescription: string = '';
  orderDescription: string = '';
  status: string = '';
  branchId?: number = 0;
  recordDate?: string = '';
  fileNo: string = '';
  judgeName: string = '';
  //closeDate?: string = '';
  //pronouncingJudgmentDate?: Date;
  receivingJudgmentDate?: Date;

  caseGrounds: CaseGrounds[] = [];
  attachments?: Attachment[];
  notes: string = '';
}

export class ObjectionCase {

  Id: number = 0;
  caseSource: number = 0;
  caseSourceNumber: string = '';
  courtId: number = 0;
  circleNumber: string = '';
  relatedCaseId: number;
  LitigationType: number

}

export class MainCaseDetails {
  id: number = 0;
  raselRef: string = '';
  raselUnifiedNo: string = '';
  caseNumberInSource: string = '';
  najizRef: string = '';
  najizId: string = '';
  moeenRef: string = '';
  caseSource: KeyValuePairs = { id: 0, name: '' };
  litigationType: KeyValuePairs = { id: 0, name: '' };
  referenceCaseNo: string = '';
  mainNo: string = '';
  startDate!: Date;
  courtId: number = 0
  circleNumber: string = '';
  subject: string = '';
  legalStatus: KeyValuePairs = { id: 0, name: '' };
  caseDescription: string = '';
  orderDescription: string = '';
  status: KeyValuePairs = { id: 0, name: '' };
  branchId?: number = 0;
  recordDate?: string = '';
  fileNo: string = '';
  judgeName: string = '';
  subCategory?: SecondSubCategory;
  receivingJudgmentDate?: Date;
  court: KeyValuePairs = { id: 0, name: '' };
  caseParties?: CasePartyDetails[] = [];
  caseGrounds?: any[] = [];
  caseMoamalat?: any[] = [];
  attachments?: Attachment[] = [];
  hearings?: HearingDetails[] = [];
  notes: string = '';
  relatedCaseId?: number;
  startDateHigri: string = '';
  relatedCase!: CaseListItem;
  moamalaId?: number;
}

export class SecondSubCategory {
  id: number;
  name: string;
  FirstSubCategoryId: number;
  firstSubCategory: FirstSubCategory;
  mainCategory: MainCategory;
}

export class FirstSubCategory {
  id: number;
  name: string;
  MainCategoryId: number;
}

export class MainCategory {
  id: number;
  name: string;
}
