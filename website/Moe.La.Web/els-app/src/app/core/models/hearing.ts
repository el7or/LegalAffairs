import { CaseListItem, CaseDetails } from "./case";
import { CaseSupportingDocumentRequest } from './supporting-document-request';
import { KeyValuePairs } from './key-value-pairs';
import { Attachment } from './attachment';
import { RequestList } from './request';
import { Guid } from 'guid-typescript';
import { HearingStatus } from "../enums/HearingStatus";


export interface HearingListItem {
  id: number;
  case: CaseListItem;
  court: string;
  status: string;
  type:string;
  hearingNumber?: number;
  hearingDate: string;
  hearingTime?: string;
  hearingDesc: string;
  circleNumber: string;
  assignedTo?: KeyValuePairs<string>;
  closingReport?: string;
  deocumentRequest?: CaseSupportingDocumentRequest;
  createdOnHigri?:string;
}

export class HearingDetails {
  id: number = 0;
  case: CaseDetails = null!;
  assignedTo: KeyValuePairs<string> = { id: '', name: '' };
  court: KeyValuePairs = { id: 0, name: '' };
  status: KeyValuePairs = { id: 0, name: '' };
  type: KeyValuePairs = { id: 0, name: '' };
  hearingNumber?: number;
  hearingDate!: Date;
  hearingDateHigri: string = '';
  hearingTime: string = '';
  hearingDesc: string = '';
  motions: string = '';
  circleNumber: string = '';
  summary?: string;
  attendees?:string;
  isPronouncedJudgment?: boolean;
  //closed?: boolean;
  closingReport?: string;
  //legalMemoId: number = 0;
  //legalMemo: any;
  hearingLegalMemoReviewRequest?: HearingLegalMemoReviewRequest;
  legalMemos!:KeyValuePairs[];
  caseSupportingDocumentRequests: RequestList[]=[];
  sessionMinutes:string="";
  isEditable:boolean=true;
  isHasNextHearing:boolean;
  attachments: Attachment[]=[];
  createdOnHigri?:string;

}

export class HearingLegalMemoReviewRequest {
  Id?: number;

  HearingId: number = 0;

  LegalMemoId: number = 0;
  legalMemo?: any;
  request?:any;
  replyDate?:any;
  replyDateHigri?:string='';
  isDeleted?:any;
}


export class SaveHearing {
  id: number = 0;
  caseId: number = 0;
  courtId : number = 0
  assignedToId?: string;
  status: HearingStatus;
  type:string='';
  hearingNumber?: number;
  hearingDate?: string;
  hearingTime?: string;
  hearingDesc?: string;
  motions?: string;
  circleNumber: string = '';
  summary?: string;
  isPronouncedJudgment?: boolean;
  closingReport?: string;
  attendees?:string;
  sessionMinutes?:string;
  withNewHearing:boolean=false;
  newHearing?:SaveHearing;
  attachments?: Attachment[];
}
export interface ReceivingJudgment {
  caseId: number;
  hearingId: number;
  hearingDate: Date;
  isPronouncedJudgment?: boolean;
  pronouncingJudgmentDate?: Date;
  receivingJudgmentDate?: Date;
}

export interface SaveAndCloseHearing {
  currentHearingId: number;
  closingReport: string;
  newHearing: SaveHearing;
}

export class HearingUpdateListItem {
  id: number=0;
  hearingId?: number;
  hearing?: any;
  text: string='';
  updateDate?: Date;
  updateDateHijri:string='';
  updateTime: string='';
  CreatedBy?:KeyValuePairs<Guid>;
  attachments?: Attachment[];
  attachment?:KeyValuePairs<string>;
}

export class HearingUpdateDetails {
  id: number=0;
  hearingId?: number;
  hearing?: HearingDetails;
  text: string='';
  updateDate?: Date;
  updateDateHijri:string='';
  updateTime: string='';
  CreatedBy?:KeyValuePairs<Guid>;
  attachments?: Attachment[];
}

export class HearingUpdateDto {
  id: number=0;
  hearingId?: number;
  text: string='';
  attachments:Attachment[] = [];
}
